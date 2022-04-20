using System.Diagnostics;
using YdkTester.Runner;

namespace YdkTesterNamespace;

public class Program
{
    public static void Main(string[] args)
    {
        var options = new Options
        {
            DeckPath = Cards.DeckPath,
            NumberOfIterations = 100000
        };

        if (Debugger.IsAttached)
        {
            options.Debug = true;
            options.NumberOfIterations = 10;
        }

        Runner.Run(options);
    }
}
