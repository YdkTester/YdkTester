using YdkTester;

namespace IceJadeTest;

public class CradleLockCondition : BaseLockCondition
{
    public override string Name => "Cradle Lock";

    public override bool CheckLockCondition(CardSet deck, CardSet hand, bool aegirineUsed)
    {
        if (hand.HasCard(Cards.FoolishBurialGoods) && deck.HasCard(Cards.IceBarrier))
            hand.AddCard(Cards.IcejadeKosmochlor);

        var hasCradle = hand.UseCard(Cards.IcejadeCenoteEnionCradle);
        if (!hasCradle && !aegirineUsed)
        {
            if (hand.UseCard(Cards.IcejadeAegirine) || hand.UseCard(Cards.IcejadeCradle))
            {
                aegirineUsed = true;
                hasCradle = true;
            }
        }

        if (!hasCradle)
            return false;

        var hasIceJadeOnField = aegirineUsed;
        if (!hasIceJadeOnField)
        {
            if (hand.HasCard(Cards.IcejadeKosmochlor))
                return true;

            if (hand.HasCard(_summonableIceJadeMonster))
                return true;

            if (hand.UseCard(Cards.IcejadeRanAegirine) && hand.HasCard(_icejadeOrWater))
                return true;

            if (hand.UseCard(Cards.IcejadeTremora) && hand.HasCard(_iceJadeMonster))
                return true;
        }

        return hasIceJadeOnField;
    }
}
