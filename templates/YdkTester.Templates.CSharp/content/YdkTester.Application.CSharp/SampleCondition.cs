using YdkTester;

namespace YdkTesterNamespace;

public class SampleCondition : ICondition
{
    public string Name => "Sample Condition";

    public bool Check(Deck deck, CardSet hand)
    {
        return true;
    }
}
