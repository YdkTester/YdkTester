
namespace YdkTester;

public interface ICondition
{
    string Name { get; }

    bool Check(CardSet deck, CardSet hand);
}
