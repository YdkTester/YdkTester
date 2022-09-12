using System.Reflection;
using CommandLine;

namespace YdkTester.Runner;

public class Runner
{
    public static void Run(params string[] args)
        => Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => Run(o));

    class RunnerThread
    {
        private Thread? _thread;

        public RunnerThread(Options options, List<ICondition> conditions, List<IGroupCondition> groupConditions)
        {
            Options = options;
            Conditions = conditions;
            GroupConditions = groupConditions;

            NormalChecks = new Dictionary<ICondition, int>();
            GroupChecks = new Dictionary<IGroupCondition, int>();
            GroupArguments = new Dictionary<Type, bool>();
        }

        public Dictionary<ICondition, int> NormalChecks { get; }
        public Dictionary<IGroupCondition, int> GroupChecks { get; }
        public Dictionary<Type, bool> GroupArguments { get; }
        public Options Options { get; }
        public List<ICondition> Conditions { get; }
        public List<IGroupCondition> GroupConditions { get; }

        public void Start(Deck deck, int iterations)
        {
            if (_thread == null)
            {
                _thread = new Thread(new ThreadStart(() => RunIterations(deck, iterations)));
                _thread.Start();
            }
        }

        public void Wait()
        {
            if (_thread == null)
                return;

            _thread.Join();
        }

        private void RunIterations(Deck deck, int iterations)
        {
            foreach (var check in Conditions)
                NormalChecks[check] = 0;

            foreach (var check in GroupConditions)
                GroupChecks[check] = 0;

            for (int x = 0; x < iterations; x++)
            {
                var deckToSend = deck.MainDeck;

                CardSet hand = Options.StartingHand == null ? 
                    deckToSend.DrawCards(5) : new CardSet(deckToSend.CardReference);

                if (Options.StartingHand != null)
                {
                    foreach (var card in Options.StartingHand)
                    {
                        deckToSend.RemoveCard(card);
                        hand.AddCard(card);
                    }
                }

                if (Options.Debug)
                    Console.WriteLine($"[Hand {hand}]");

                foreach (var normalCondition in Conditions)
                {
                    var isCheckSuccessful = normalCondition.Check(deckToSend, hand);
                    GroupArguments[normalCondition.GetType()] = isCheckSuccessful;

                    if (isCheckSuccessful)
                        NormalChecks[normalCondition]++;

                    if (Options.Debug)
                        Console.WriteLine($"{normalCondition.Name}: {isCheckSuccessful}");
                }

                if (Options.Debug)
                    Console.WriteLine("");

                foreach (var groupCondition in GroupConditions)
                    if (groupCondition.Check(GroupArguments))
                        GroupChecks[groupCondition]++;
            }
        }
    }

    public static void Run(Options options)
    {
        // TODO: Implement ygoprodeck.com API as an alternative tester
        var cardResolver = new YdkTester.Modules.EdoPro.EdoProCardResolver(options.EdoProPath);

        var deck = YdkDeckParser.Parse(options.DeckPath, cardResolver);

        var conditions = new List<ICondition>();
        var groupCounditions = new List<IGroupCondition>();
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in loadedAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (typeof(ICondition).IsAssignableFrom(type))
                {
                    conditions.Add((ICondition)Activator.CreateInstance(type)!);
                }

                if (typeof(IGroupCondition).IsAssignableFrom(type))
                {
                    groupCounditions.Add((IGroupCondition)Activator.CreateInstance(type)!);
                }
            }
        }

        if (options.StartingHand != null)
            options.NumberOfIterations = 1;

        var threadCount = options.Debug ? 1 : Environment.ProcessorCount - 1;
        var runnerThreads = new RunnerThread[threadCount];
        var iterations = options.NumberOfIterations / threadCount;
        var extraIterations = options.NumberOfIterations % threadCount;

        for (int i = 0; i < runnerThreads.Length; i++)
        {
            runnerThreads[i] = new RunnerThread(options, conditions, groupCounditions);
            runnerThreads[i].Start(deck, i == 0 ? (iterations + extraIterations) : iterations);
        }

        for (int i = 0; i < runnerThreads.Length; i++)
            runnerThreads[i].Wait();

        Console.WriteLine("[Conditions]");
        foreach (var pair in runnerThreads[0].NormalChecks)
        {
            var value = pair.Value;

            for (int i = 1; i < runnerThreads.Length; i++)
                value += runnerThreads[i].NormalChecks[pair.Key];

            Write(pair.Key.Name, pair.Value, options.NumberOfIterations);
        }
        Console.WriteLine("");

        Console.WriteLine("[Groups]");
        foreach (var pair in runnerThreads[0].GroupChecks)
        {
            var value = pair.Value;

            for (int i = 1; i < runnerThreads.Length; i++)
                value += runnerThreads[i].GroupChecks[pair.Key];

            Write(pair.Key.Name, value, options.NumberOfIterations);
        }
    }

    private static void Write(string title, int successCases, int totalCases)
    {
        var successPercentage = successCases * 100.0 / totalCases;
        Console.WriteLine(title + ": " + successPercentage.ToString("0.##") + "% (" + successCases + " / " + totalCases + ")");
    }
}
