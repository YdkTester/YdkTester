
namespace YdkTester;

public class CardAction
{
    public delegate bool CardActionDelegate(Card card);

    private static int _id;

    private CardActionDelegate _action;

    public CardAction(CardActionDelegate action)
    {
        Id = Interlocked.Increment(ref _id);
        _action = action;
    }

    public int Id { get; }

    public bool Check(Card card)
        => _action(card);
}
