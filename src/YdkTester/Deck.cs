
namespace YdkTester;

public struct Deck
{
    private CardSet _mainDeck, _extraDeck, _sideDeck;

    public Deck(CardReference cardReference, int mainDeck, int extraDeck, int sideDeck)
    {
        _mainDeck = new CardSet(cardReference, 0, (byte)mainDeck);
        _extraDeck = new CardSet(cardReference, (byte)(mainDeck), (byte)(mainDeck + extraDeck));
        _sideDeck = new CardSet(cardReference, (byte)(mainDeck + extraDeck), (byte)(mainDeck + extraDeck + sideDeck));
    }

    public CardSet MainDeck => _mainDeck;

    public CardSet ExtraDeck => _extraDeck;

    public CardSet SideDeck => _sideDeck;

    public List<Card> Cards
    {
        get
        {
            var ret = new List<Card>();

            ret.AddRange(_mainDeck.Cards);
            ret.AddRange(_extraDeck.Cards);
            ret.AddRange(_sideDeck.Cards);

            return ret;
        }
    }
}
