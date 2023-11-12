using YdkTester;

namespace ReptilianneTest;

public class CosmicSlicerLockCondition : ICondition
{
    public string Name => "Cosmic Slicer Lock";

    private static CardAction _summonableReptileExceptKageAndCoatl = new(c => c != Cards.Kagetokage && c != Cards.ReptilianneCoatl && c.Category.HasFlag(CardCategory.Monster) && c.Level == 4 && c.Type == CardType.Reptile);
    private static CardAction _nonRamificatorsST = new(c => c != Cards.ReptilianneRamifications && c.Title.Contains("Reptilianne") && (c.Category.HasFlag(CardCategory.Spell) || c.Category.HasFlag(CardCategory.Trap)));
    private static CardAction _nonNunuReptileMonster = new(c => c.Type == CardType.Reptile && c != Cards.NunutheOgdoadicRemnant);
    private static CardAction _summonableMonster = new(c => c.Category.HasFlag(CardCategory.Monster) && c.Level <= 4);
    private static CardAction _darkReptileMonster = new(c => c.Type == CardType.Reptile && c.Level == 4 && c.Attribute == CardAttribute.Dark);

    public bool Check(CardSet deck, CardSet hand)
    {
        if (hand.HasCard(Cards.SnakeRain) || hand.HasCard(Cards.NunutheOgdoadicRemnant))
            return true;

        if (hand.HasCard(Cards.SmallWorld) && (hand.HasCard(Cards.EffectVeiler) || hand.HasCard(_nonNunuReptileMonster)))
            return true;

        if (CheckNauyaCombos(deck, hand))
            return true;

        if (CheckCoatlCombos(deck, hand))
            return true;

        return false;
    }

    private static bool CheckNauyaCombos(CardSet deck, CardSet hand)
    {
        if (hand.UseCard(Cards.NauyatheOgdoadicRemnant))
            return false;

        if (hand.HasCard(_summonableMonster))
            return true;

        if (hand.HasCard(Cards.ReptilianneRamifications) && deck.HasCard(_nonRamificatorsST))
            return true;

        return false;
    }

    private static bool CheckCoatlCombos(CardSet deck, CardSet hand)
    {
        if (!hand.UseCard(Cards.ReptilianneCoatl))
            return false;

        if (hand.HasCard(_darkReptileMonster))
            return true;

        if (hand.HasCard(Cards.ReptilianneRamifications) && deck.HasCard(_nonRamificatorsST))
            return true;

        return false;
    }
}
