using YdkTester;

namespace ReptilianneTest;

public class ProtectionCondition : ICondition
{
    public string Name => "Contains Protection";

    public bool Check(Deck deck, CardSet hand)
    {
        return hand.HasCard(Cards.InstantFusion) ||
            hand.HasCard(Cards.CalledbytheGrave) ||
            hand.HasCard(Cards.CrossoutDesignator) ||
            hand.HasCard(Cards.PSYFramegearGamma);
    }
}
