using CommandLine;

namespace YdkTester.Generator;

enum Language
{
    CS,
    FS
}

class Options
{
    public Options()
    {
        DeckPath = "";
        Namespace = "";
        OutputPath = "";
        Language = Language.CS;

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

    [Option("language", Required = false, HelpText = "Sets the programming language used for the generator (cs or fs)")]
    public Language Language { get; set; }

    [Option("namespace", Required = true, HelpText = "Sets the namespace to use when generating Cards.cs")]
    public string Namespace { get; set; }

    [Option('o', "outputPath", Required = true, HelpText = "Sets the path to Cards.cs")]
    public string OutputPath { get; set; }
}
