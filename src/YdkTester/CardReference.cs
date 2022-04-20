
namespace YdkTester;

public class CardReference
{
    private Dictionary<int, List<byte>> _cardIdResolver;
    private List<Card> _cards;
    private static List<byte> _emptyArray = new List<byte>();

    public CardReference(List<Card> cards)
    {
        _cardIdResolver = new Dictionary<int, List<byte>>();
        _cards = cards;

        for (int i = 0; i < cards.Count; i++)
        {
            List<byte>? positions;

            if (!_cardIdResolver.TryGetValue(cards[i].Id, out positions))
            {
                positions = new List<byte>();
                _cardIdResolver[cards[i].Id] = positions;
            }

            positions?.Add((byte)i);
        }
    }

    public List<byte> ResolveId(int id)
    {
        if (_cardIdResolver.TryGetValue(id, out List<byte>? ret))
            if (ret != null)
                return ret;

        return _emptyArray;
    }

    public Card GetCard(int offset)
    {
        return _cards[offset];
    }
}
