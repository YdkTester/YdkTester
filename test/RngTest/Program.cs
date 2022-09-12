using System.Diagnostics;
using YdkTester.Runner;

namespace RngTest;

public class Program
{
    public static void Main(string[] args)
    {
        var options = new Options
        {
            DeckPath = Cards.DeckPath,
            NumberOfIterations = 1000000
        };

        if (Debugger.IsAttached)
        {
            options.Debug = true;
            options.NumberOfIterations = 10;
        }

        Runner.Run(options);
        
        Console.WriteLine("");
        Console.WriteLine("[Cards]");

        var counter = 0;
        var min = options.NumberOfIterations;
        var max = 0;

        foreach (var pair in RngTestCondition.CardCounter)
        {
            Console.Write(pair.Value + " ");

            if (pair.Value < min)
                min = pair.Value;

            if (pair.Value > max)
                max = pair.Value;

            counter++;

            if (counter % 10 == 0)
                Console.Write("\n");
        }

        Console.Write("\n");
        Console.WriteLine("[Stats]");
        Console.WriteLine("Count: " + counter);
        Console.WriteLine("Min: " + min);
        Console.WriteLine("Max: " + max);
        Console.WriteLine("Diff: " + (max - min));
    }
}
