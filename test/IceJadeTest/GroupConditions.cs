using YdkTester;

namespace IceJadeTest;

public class LockCondition : IGroupCondition
{
    public string Name => "Lock";

    public bool Check(Dictionary<Type, bool> conditions)
    {
        return conditions[typeof(CradleLockCondition)] || conditions[typeof(RanLockCondition)];
    }
}
