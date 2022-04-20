
namespace YdkTester;

public interface IGroupCondition
{
    string Name { get; }

    bool Check(Dictionary<Type, bool> conditions);
}
