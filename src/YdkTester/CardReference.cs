
namespace YdkTester;

public class CardReference
{
    private static readonly byte[] _emptyArray = [];

    private readonly Dictionary<int, CardSetArray> _cardArrayResolver;
    private readonly Dictionary<int, CardSetData> _cardIdResolver;
    private readonly Dictionary<int, CardSetData> _cardActionResolver;
    private readonly List<Card> _cards;

    public CardReference(List<Card> cards)
    {
        _cardArrayResolver = [];
        _cardIdResolver = [];
        _cardActionResolver = [];
        _cards = cards;

        for (int i = 0; i < cards.Count; i++)
        {
            if (!_cardIdResolver.TryGetValue(cards[i].Id, out CardSetData data))
                data = new CardSetData();
            data.SetBitOn(i);
            _cardIdResolver[cards[i].Id] = data;

            if (!_cardArrayResolver.TryGetValue(cards[i].Id, out CardSetArray arr))
                arr = new CardSetArray();
            arr.Add(i);
            _cardArrayResolver[cards[i].Id] = arr;
        }
    }

    public CardSetData ResolveId(int id)
    {
        if (!_cardIdResolver.TryGetValue(id, out CardSetData data))
            data = new CardSetData();
        
        return data;
    }

    public CardSetArray ResolveArray(int id)
    {
        if (!_cardArrayResolver.TryGetValue(id, out CardSetArray arr))
            arr = new CardSetArray();
        
        return arr;
    }

    public CardSetData ResolveAction(CardAction action)
    {
        if (!_cardActionResolver.TryGetValue(action.Id, out CardSetData data))
        {
            data = new CardSetData();

            for (int i = 0; i < _cards.Count; i++)
            {
                if (action.Check(_cards[i]))
                {
                    data.SetBitOn(i);
                }
            }

            _cardActionResolver[action.Id] = data;
        }

        return data;
    }

    public Card GetCard(int offset)
    {
        return _cards[offset];
    }
}
