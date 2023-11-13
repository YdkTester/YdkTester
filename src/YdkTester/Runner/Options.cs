using CommandLine;

namespace YdkTester.Runner;

public class Options
{
    public static Options Instance { get; set; } = new Options();

    public Options()
    {
        Instance = this;

        DeckPath = "";
        NumberOfIterations = 100000;
        
        if (OperatingSystem.IsWindows())
            EdoProPath = @"C:\ProjectIgnis";
        else if (OperatingSystem.IsMacOS())
            EdoProPath = "/Applications/ProjectIgnis";
        else
            EdoProPath = "/opt/edopro";
    }

    [Option("deckPath", Required = true, HelpText = "Sets the path of the deck to parse the cards from")]
    public string DeckPath { get; set; }

    [Option("edoProPath", Required = false, HelpText = "Sets the path to EdoPro")]
    public string EdoProPath { get; set; }

    [Option("iterations", Required = false, HelpText = "Number of iterations to run")]
    public int NumberOfIterations { get; set; }

    [Option("debug", Required = false, HelpText = "Prints out each hand and if the conditions were successful")]
    public bool Debug { get; set; }

    public Card[]? StartingHand { get; set; }
}
