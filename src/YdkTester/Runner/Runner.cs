using System.Reflection;
using CommandLine;

namespace YdkTester.Runner;

public class Runner
{
    public static void Run(params string[] args)
        => Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => Run(o));

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

        var _normalChecks = new Dictionary<ICondition, int>();
        var _groupChecks = new Dictionary<IGroupCondition, int>();
        var _groupArguments = new Dictionary<Type, bool>();

        foreach (var check in conditions)
            _normalChecks[check] = 0;

        foreach (var check in groupCounditions)
            _groupChecks[check] = 0;

        for (int x = 0; x < options.NumberOfIterations; x++)
        {
            var deckToSend = deck;
            var hand = deckToSend.MainDeck.DrawOpeningHand();

            if (options.Debug)
                Console.WriteLine($"[Hand {hand}]");

            foreach (var normalCondition in conditions)
            {
                var isCheckSuccessful = normalCondition.Check(deckToSend, hand);
                _groupArguments[normalCondition.GetType()] = isCheckSuccessful;

                if (isCheckSuccessful)
                    _normalChecks[normalCondition]++;

                if (options.Debug)
                    Console.WriteLine($"{normalCondition.Name}: {isCheckSuccessful}");
            }

            if (options.Debug)
                Console.WriteLine("");

            foreach (var groupCondition in groupCounditions)
                if (groupCondition.Check(_groupArguments))
                    _groupChecks[groupCondition]++;
        }

        Console.WriteLine("[Conditions]");
        foreach (var pair in _normalChecks)
        {
            Write(pair.Key.Name, pair.Value, options.NumberOfIterations);
        }
        Console.WriteLine("");

        Console.WriteLine("[Groups]");
        foreach (var pair in _groupChecks)
        {
            Write(pair.Key.Name, pair.Value, options.NumberOfIterations);
        }
    }

    private static void Write(string title, int successCases, int totalCases)
    {
        var successPercentage = successCases * 100.0 / totalCases;
        Console.WriteLine(title + ": " + successPercentage.ToString("0.##") + "% (" + successCases + " / " + totalCases + ")");
    }
}
