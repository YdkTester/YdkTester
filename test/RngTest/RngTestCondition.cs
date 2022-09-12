using YdkTester;

namespace RngTest;

public class RngTestCondition : ICondition
{
    private static bool _init = false;
    public static Dictionary<int, int> CardCounter = new Dictionary<int, int>();

    public string Name => "Rng Test";

    public bool Check(Deck deck, CardSet hand)
    {
        if (!_init)
        {
            foreach (var card in deck.MainDeck.Cards)
                CardCounter[card.Id] = 0;

            foreach (var card in hand.Cards)
                CardCounter[card.Id] = 0;
            
            _init = true;
        }

        foreach (var card in hand.Cards)
            CardCounter[card.Id]++;

        return true;
    }
}
