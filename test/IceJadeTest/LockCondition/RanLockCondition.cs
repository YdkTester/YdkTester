using YdkTester;

namespace IceJadeTest;

public class RanLockCondition : BaseLockCondition
{
    public override string Name => "Ran Lock";

    public override bool CheckLockCondition(CardSet deck, CardSet hand, bool aegirineUsed)
    {
        var iceBarrierUsed = false;

        // Find Argirine
        if (!hand.UseCard(Cards.IcejadeRanAegirine))
        {
            if (!aegirineUsed && hand.UseCard(Cards.IcejadeAegirine))
            {
                aegirineUsed = true;
            }
            else if (hand.HasCard(Cards.FoolishBurialGoods) && deck.HasCard(Cards.IceBarrier))
            {
                iceBarrierUsed = true;
            }
            else if (hand.UseCard(Cards.IcejadeCradle))
            {

            }
            else
            {
                return false;
            }
        }

        // Find Aegirocasis
        if (hand.HasCard(Cards.IcejadeCreationAegirocassis))
            return true;

        if (hand.HasCard(Cards.IcejadeCradle))
            return true;

        if (!aegirineUsed && hand.HasCard(Cards.IcejadeAegirine))
            return true;

        if (!iceBarrierUsed && hand.HasCard(Cards.FoolishBurialGoods) && deck.HasCard(Cards.IceBarrier))
            return true;

        // Check for the other creations, that means we also need to discard another card for cost
        return hand.UseCard(Cards.IcejadeCreationKingfisher) && hand.HasCard(_icejadeOrWater);
    }
}
