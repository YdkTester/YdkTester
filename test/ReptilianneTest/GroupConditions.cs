using YdkTester;

namespace ReptilianneTest;

public class FirstHandCondition : IGroupCondition
{
    public string Name => "First Hand";

    public bool Check(Dictionary<Type, bool> conditions)
    {
        return conditions[typeof(ReptilianneDrawCondition)] || conditions[typeof(CosmicSlicerLockCondition)];
    }
}

public class FirstHandWithProtectionCondition : IGroupCondition
{
    private static FirstHandCondition _firstHandCondition = new FirstHandCondition();

    public string Name => "First Hand With Protection";

    public bool Check(Dictionary<Type, bool> conditions)
    {
        return _firstHandCondition.Check(conditions) && conditions[typeof(ProtectionCondition)];
    }
}
