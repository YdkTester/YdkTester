
using System.Diagnostics;
using YdkTester.Runner;

namespace YdkTester;

public abstract class SmartCondition : ICondition
{
    public abstract string Name { get; }

    public bool Check(CardSet deck, CardSet hand)
    {
        if (hand.UseCard(84211599))
        {
            CardSet revealedCards = deck.DrawCards(6);
            
            if (Options.Instance.Debug)
                Console.WriteLine($"[Prosperity {revealedCards}]");

            for (int i = 0; i < 6; i++)
            {
                var card = revealedCards.DrawCard();

                // Put card in hand and run the checks
                hand.AddCard(card);
                if (CheckWithRevealed(deck, hand, revealedCards))
                    return true;

                // Return card to the deck
                hand.RemoveCard(card);
                deck.AddCard(card);
            }
        }

        return CheckInternal(deck, hand);
    }

    private bool CheckWithRevealed(CardSet deck, CardSet hand, CardSet revealedCards)
    {
        while (revealedCards.Count > 0)
            deck.AddCard(revealedCards.DrawCard());

        return CheckInternal(deck, hand);
    }

    protected abstract bool CheckInternal(CardSet deck, CardSet hand);
}
