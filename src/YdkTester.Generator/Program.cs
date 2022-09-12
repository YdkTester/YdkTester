﻿using System.Text.RegularExpressions;
using CommandLine;

namespace YdkTester.Generator;

static class Program
{
    public static void Main(string[] args)
        => (new Parser(with => with.CaseInsensitiveEnumValues = true)).ParseArguments<Options>(args).WithParsed<Options>(o => Run(o));

    private static void Run(Options options)
    {
        Console.WriteLine(typeof(Program).Assembly.Location);

        // TODO: Implement ygoprodeck.com API as an alternative tester
        var cardResolver = new YdkTester.Modules.EdoPro.EdoProCardResolver(options.EdoProPath);

        var cardDict = new Dictionary<int, Card>();

        if (File.Exists(options.OutputPath))
        {
            var text = File.ReadAllText(options.OutputPath);
            var cspattern = @"Card { Id = (?<id>[0-9]+), Title = ""(?<title>[^""]+)"", Category = \(CardCategory\)(?<type>[0-9]+), Atk = (?<atk>[0-9]+), Def = (?<def>[0-9]+), Level = (?<level>[0-9]+), Type = \(CardType\)(?<race>[0-9]+), Attribute = \(CardAttribute\)(?<attribute>[0-9]+) };";
            var fspattern = @"Card\(Id = (?<id>[0-9]+), Title = ""(?<title>[^""]+)"", Category = enum\<CardCategory\>(?<type>[0-9]+), Atk = (?<atk>[0-9]+), Def = (?<def>[0-9]+), Level = (?<level>[0-9]+), Type = enum\<CardType\>(?<race>[0-9]+), Attribute = enum\<CardAttribute\>(?<attribute>[0-9]+)\)";
            var pattern = options.Language == Language.CS ? cspattern : fspattern;

            foreach (Match match in Regex.Matches(text, pattern, RegexOptions.None))
            {
                var id = int.Parse(match.Groups["id"]?.ToString()!);
                var title = match.Groups["title"]?.ToString() ?? "";
                var type = int.Parse(match.Groups["type"]?.ToString()!);
                var atk = int.Parse(match.Groups["atk"]?.ToString()!);
                var def = int.Parse(match.Groups["def"]?.ToString()!);
                var level = int.Parse(match.Groups["level"]?.ToString()!);
                var race = int.Parse(match.Groups["race"]?.ToString()!);
                var attribute = int.Parse(match.Groups["attribute"]?.ToString()!);

                cardDict[id] = new Card
                {
                    Id = id,
                    Title = title,
                    Category = (CardCategory)type,
                    Atk = atk,
                    Def = def,
                    Level = level,
                    Type = (CardType)race,
                    Attribute = (CardAttribute)attribute
                };
            }
        }

        var deck = YdkDeckParser.Parse(options.DeckPath, cardResolver);

        foreach (var card in deck.Cards)
            cardDict[card.Id] = card;

        var lines = new List<string>();

        lines.Add("// ######################################");
        lines.Add("// ##### THIS FILE IS AUTOGENERATED #####");
        lines.Add("// ##### DO NOT MODIFY IT BY HAND #######");
        lines.Add("// ######################################");
        lines.Add("");

        if (options.Language == Language.CS)
        {
            lines.Add("using YdkTester;");
            lines.Add("");
            lines.Add($"namespace {options.Namespace};");
            lines.Add("");
            lines.Add("public class Cards");
            lines.Add("{");
            lines.Add($"    public const string DeckPath = \"{options.DeckPath}\";");
            lines.Add("");
        }
        else
        {
            lines.Add($"namespace {options.Namespace}");
            lines.Add("");
            lines.Add("open YdkTester");
            lines.Add("");
            lines.Add("type Cards =");
            lines.Add($"    static member DeckPath : string = \"{options.DeckPath}\"");
            lines.Add("");
        }

        foreach (var card in cardDict)
        {
            var cardText = "";

            if (options.Language == Language.CS)
            {
                cardText += $"    public static readonly Card {GetSafeName(card.Value.Title)} = new Card";
                cardText += " { ";
                cardText += $"Id = {card.Key}, ";
                cardText += $"Title = \"{card.Value.Title.Replace("\"", "\\\"")}\", ";
                cardText += $"Category = (CardCategory){(int)card.Value.Category}, ";
                cardText += $"Atk = {card.Value.Atk}, ";
                cardText += $"Def = {card.Value.Def}, ";
                cardText += $"Level = {card.Value.Level}, ";
                cardText += $"Type = (CardType){(int)card.Value.Type}, ";
                cardText += $"Attribute = (CardAttribute){(int)card.Value.Attribute}";
                cardText += " };";
            }
            else
            {
                cardText += $"    static member {GetSafeName(card.Value.Title)} : Card = new Card(";
                cardText += $"Id = {card.Key}, ";
                cardText += $"Title = \"{card.Value.Title.Replace("\"", "\\\"")}\", ";
                cardText += $"Category = enum<CardCategory>{(int)card.Value.Category}, ";
                cardText += $"Atk = {card.Value.Atk}, ";
                cardText += $"Def = {card.Value.Def}, ";
                cardText += $"Level = {card.Value.Level}, ";
                cardText += $"Type = enum<CardType>{(int)card.Value.Type}, ";
                cardText += $"Attribute = enum<CardAttribute>{(int)card.Value.Attribute}";
                cardText += ")";
            }

            lines.Add(cardText);
            lines.Add("");
        }

        if (options.Language == Language.CS)
            lines.Add("}");

        var dir = Path.GetDirectoryName(options.OutputPath) ?? "";

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllLines(options.OutputPath, lines);
    }

    public static string GetSafeName(string title)
    {
        var ret = "";

        foreach (var c in title)
        {
            if (string.IsNullOrEmpty(ret) && char.IsNumber(c))
                ret += "_";

            if (char.IsLetter(c) || char.IsNumber(c))
                ret += c;
        }

        return ret;
    }
}
