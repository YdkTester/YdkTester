using YdkTester;

namespace IceJadeTest;

public abstract class BaseLockCondition : SmartCondition
{
    protected static readonly CardAction _icejadeOrWater = new(c => c.Title.ToLower().Contains("icejade") || (c.Category.HasFlag(CardCategory.Monster) && c.Attribute == CardAttribute.Water));
    protected static readonly CardAction _summonableIceJadeMonster = new(c => c.Title.ToLower().Contains("icejade") && c.Category.HasFlag(CardCategory.Monster) && c.Level <= 4);
    protected static readonly CardAction _iceJadeMonster = new(c => c.Title.ToLower().Contains("icejade") && c.Category.HasFlag(CardCategory.Monster));

    protected override bool CheckInternal(CardSet deck, CardSet hand)
    {
        bool aegirineUsed = false;

        // We can convert a deep sea diva to an Aegirine
        if (hand.UseCard(Cards.DeepSeaDiva))
            hand.AddCard(Cards.IcejadeAegirine);

        if (hand.UseCard(Cards.Terraforming))
            hand.AddCard(Cards.IcejadeCenoteEnionCradle);

        // Check for Curse, its mandatory for the lock!
        if (!hand.UseCard(Cards.IcejadeCurse))
        {
            if (hand.UseCard(Cards.IcejadeAegirine) || hand.UseCard(Cards.IcejadeCradle))
            {
                aegirineUsed = true;
            }
            else
            {
                // We don't have curse, we can't lock
                return false;
            }
        }

        if (CheckLockCondition(deck, hand, aegirineUsed))
            return true;

        return false;
    }

    public abstract bool CheckLockCondition(CardSet deck, CardSet hand, bool aegirineUsed);
}
