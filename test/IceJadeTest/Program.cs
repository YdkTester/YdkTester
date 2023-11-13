using System.Diagnostics;
using YdkTester.Runner;

namespace IceJadeTest;

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

        //options.StartingHand = new[] { };

        Runner.Run(options);
    }
}
