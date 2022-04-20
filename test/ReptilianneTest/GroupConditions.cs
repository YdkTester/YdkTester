using YdkTester;

namespace ReptilianneTest;

public class FirstHandCondition : IGroupCondition
{
    public string Name => "First Hand";

    public bool Check(Dictionary<Type, bool> conditions)
    {
        return conditions[typeof(MainReptilianneCondition)];
    }
}

public class FirstHandWithProtectionCondition : IGroupCondition
{
    public string Name => "First Hand With Protection";

    public bool Check(Dictionary<Type, bool> conditions)
    {
        return conditions[typeof(MainReptilianneCondition)] && conditions[typeof(ProtectionCondition)];
    }
}
