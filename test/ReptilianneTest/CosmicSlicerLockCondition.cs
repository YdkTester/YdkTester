using YdkTester;

namespace ReptilianneTest;

public class CosmicSlicerLockCondition : ICondition
{
    public string Name => "Cosmic Slicer Lock";

    private static CardAction _summonableReptileExceptKageAndCoatl = new CardAction(c => c != Cards.Kagetokage && c != Cards.ReptilianneCoatl && c.Category.HasFlag(CardCategory.Monster) && c.Level == 4 && c.Type == CardType.Reptile);
    private static CardAction _reptilianneSpellOrTrap = new CardAction(c => c.Title.Contains("Reptilianne") && (c.Category.HasFlag(CardCategory.Spell) || c.Category.HasFlag(CardCategory.Trap)));

    public bool Check(CardSet deck, CardSet hand)
    {
        if (hand.UseCard(Cards.SnakeRain) || hand.UseCard(Cards.NunutheOgdoadicRemnant))
            return true;

        if (CheckFullLock(deck, hand))
            return true;

        if (CheckAlienKidCombo(deck, hand))
            return true;

        return false;
    }

    private bool CheckFullLock(CardSet deck, CardSet hand)
    {
        deck.AddCard(Cards.AlienStealthbuster);

        if (CheckAlienKidCombo(deck, hand))
        {
            return hand.HasCard(Cards.ReptilianneSpawn) || hand.HasCard(Cards.ReptilianneRamifications);
        }

        return false;
    }

    private bool CheckAlienKidCombo(CardSet deck, CardSet hand)
    {
        if (!deck.HasCard(Cards.AlienStealthbuster))
            return false;

        // Remvoe alien kid from hand for testing purposes
        hand.UseCard(Cards.AlienKid);

        var keurseInHand = hand.UseCard(Cards.KeursetheOgdoadicLight);

        // If zohah is in hand, we'll be able to discard keurse
        if (hand.HasCard(Cards.ZohahtheOgdoadicBoundless))
            keurseInHand = false;

        var summonCounter = 0;

        // Resolve ramifications
        if (hand.HasCard(Cards.ReptilianneRamifications) && deck.HasCard(_reptilianneSpellOrTrap))
        {
            hand.UseCard(Cards.ReptilianneRamifications);

            if (!hand.HasCard(Cards.ReptilianneCoatl))
                hand.AddCard(Cards.ReptilianneCoatl);
            else
                hand.AddCard(Cards.ReptilianneScylla);

            keurseInHand = false;
        }

        // Use foolish burial
        if (hand.UseCard(Cards.FoolishBurial))
        {
            if (keurseInHand)
                keurseInHand = false;
            else
                summonCounter++;
        }

        var coatlInHand = hand.UseCard(Cards.ReptilianneCoatl);
        var normalSummoned = false;

        // Do a normal summon of not coatl
        if (hand.HasCard(_summonableReptileExceptKageAndCoatl))
        {
            normalSummoned = true;
            summonCounter++;

            if (coatlInHand)
                summonCounter++;
        }

        // Do a normal summon of coatl
        if (!normalSummoned && coatlInHand)
        {
            normalSummoned = true;
            summonCounter++;
        }

        // Summon kagetokage if we normal summoned
        if (normalSummoned && hand.UseCard(Cards.Kagetokage))
            summonCounter++;

        return summonCounter >= 2 && !keurseInHand;
    }
}
