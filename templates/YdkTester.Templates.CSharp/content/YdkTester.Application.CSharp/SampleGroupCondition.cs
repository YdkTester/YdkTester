using YdkTester;

namespace YdkTesterNamespace;

public class SampleGroupCondition : IGroupCondition
{
    public string Name => "Sample Group Condition";

    public bool Check(Dictionary<Type, bool> conditions)
        => conditions[typeof(SampleCondition)];
}
