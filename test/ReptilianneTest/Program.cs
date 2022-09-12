using System.Diagnostics;
using YdkTester.Runner;

namespace ReptilianneTest;

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

        // options.StartingHand = new[] { Cards.NunutheOgdoadicRemnant, Cards.SnakeRain, Cards.FoolishBurial, Cards.NibiruthePrimalBeing, Cards.AlienStealthbuster };

        Runner.Run(options);
    }
}
