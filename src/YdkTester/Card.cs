
namespace YdkTester;

public struct Card
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int Atk { get; set; }

    public int Def { get; set; }

    public CardCategory Category { get; set; }

    public CardAttribute Attribute { get; set; }

    public CardType Type { get; set; }

    public int Level { get; set; }

    public override bool Equals(object? obj) => obj is Card card && card.Id == Id;

    public override int GetHashCode() => Id;

    public static bool operator ==(Card lhs, Card rhs) => lhs.Equals(rhs);

    public static bool operator !=(Card lhs, Card rhs) => !lhs.Equals(rhs);
}
