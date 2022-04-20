
namespace YdkTester;

public interface ICondition
{
    string Name { get; }

    bool Check(Deck deck, CardSet hand);
}
