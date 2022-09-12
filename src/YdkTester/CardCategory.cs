
namespace YdkTester;

[Flags]
public enum CardCategory
{
    Monster = 1,
    Spell = 2,
    Trap = 4,
    Normal = 16,
    Effect = 32,
    Fusion = 64,
    Ritual = 128,
    Spirit = 512,
    Union = 1024,
    Gemini = 2048,
    Tuner = 4096,
    Synchro = 8192,
    Token = 16384,
    QuickPlay = 65536,
    Continuous = 131072,
    Equip = 262144,
    Field = 524288,
    Counter = 1048576,
    Flip = 2097152,
    Toon = 4194304,
    Xyz = 8388608,
    Pendulum = 16777216
}
