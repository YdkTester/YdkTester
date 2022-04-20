
namespace YdkTester;

public static class YdkDeckParser
{
    private enum CardSet
    {
        MainDeck,
        ExtraDeck,
        SideDeck
    }

    public static Deck Parse(string deckPath, ICardResolver cardResolver)
    {
        var set = CardSet.MainDeck;
        var setDictionary = new Dictionary<CardSet, int>();
        setDictionary[CardSet.MainDeck] = 0;
        setDictionary[CardSet.SideDeck] = 0;
        setDictionary[CardSet.ExtraDeck] = 0;

        var lines = File.ReadAllLines(deckPath);
        var cards = new List<Card>();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            if (trimmedLine == "#main")
            {
                set = CardSet.MainDeck;
            }
            else  if (trimmedLine == "#extra")
            {
                set = CardSet.ExtraDeck;
            }
            else if (trimmedLine == "!side")
            {
                set = CardSet.SideDeck;
            }
            else if (!string.IsNullOrWhiteSpace(trimmedLine) && 
                    !trimmedLine.StartsWith("#") &&
                    !trimmedLine.StartsWith("!") &&
                    int.TryParse(trimmedLine, out int id))
            {
                cards.Add(cardResolver.Parse(id));
                setDictionary[set]++;
            }
        }

        var cardReference = new CardReference(cards);
        return new Deck(cardReference, setDictionary[CardSet.MainDeck], setDictionary[CardSet.ExtraDeck], setDictionary[CardSet.SideDeck]);
    }
}
