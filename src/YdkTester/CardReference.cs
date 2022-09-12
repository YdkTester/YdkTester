
namespace YdkTester;

public class CardReference
{
    private static byte[] _emptyArray = new byte[0];

    private Dictionary<int, byte[]> _cardIdResolver;
    private Dictionary<int, byte[]> _cardActionResolver;
    private List<Card> _cards;

    public CardReference(List<Card> cards)
    {
        _cardIdResolver = new Dictionary<int, byte[]>();
        _cardActionResolver = new Dictionary<int, byte[]>();
        _cards = cards;

        // TODO: Fix
        for (int i = 0; i < cards.Count; i++)
        {
            if (_cardIdResolver.TryGetValue(cards[i].Id, out byte[]? positions))
            {
                var posArray = positions.ToList();
                posArray.Add((byte)i);

                _cardIdResolver[cards[i].Id] = posArray.ToArray();
            }
            else
            {
                _cardIdResolver[cards[i].Id] = new byte[] { (byte)i };
            }
        }
    }

    public byte[] ResolveId(int id)
    {
        byte[]? ret;
        _cardIdResolver.TryGetValue(id, out ret);
        return ret ?? _emptyArray;
    }

    public byte[] ResolveAction(CardAction action)
    {
        byte[]? ret;

        if (!_cardActionResolver.TryGetValue(action.Id, out ret))
        {
            var validBytes = new List<byte>();

            for (int i = 0; i < _cards.Count; i++)
            {
                if (action.Check(_cards[i]))
                {
                    validBytes.Add((byte)i);
                }
            }

            ret = _cardActionResolver[action.Id] = validBytes.ToArray();
        }

        return ret ?? _emptyArray;
    }

    public Card GetCard(int offset)
    {
        return _cards[offset];
    }
}
