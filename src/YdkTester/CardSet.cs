
namespace YdkTester;

public struct CardSet
{
    private static readonly Random rng = new();

    private CardSetData _data;
    private CardReference _cardReference; // This is a reference

    public CardSet(CardReference deckReference)
    {
        _cardReference = deckReference;
    }

    public CardSet(CardReference deckReference, byte bitstart, byte bitend)
    {
        _cardReference = deckReference;

        for (byte i = bitstart; i < bitend; i++)
        {
            _data.SetBitOn(i);
        }
    }

    public readonly int Count => _data.Count;

    public readonly CardReference CardReference => _cardReference;

    public readonly List<Card> Cards
    {
        get
        {
            var ret = new List<Card>();

            for (byte i = 0; i < 96; i++)
            {
                if (_data.IsBitSet(i))
                {
                    ret.Add(_cardReference.GetCard(i));
                }
            }

            return ret;
        }
    }

    public bool AddCard(Card card)
    {
        var positions = _cardReference.ResolveArray(card.Id);

        for (int i = 0; i < 3; i++)
        {
            var position = positions.GetAt(i);
            if (position < 0)
                return false;

            if (!_data.IsBitSet(position))
            {
                _data.SetBitOn(position);
                return true;
            }
        }

        return false;
    }

    public bool RemoveCard(Card card) => RemoveCard(card.Id);

    public bool RemoveCard(int cardId)
    {
        var positions = _cardReference.ResolveArray(cardId);

        for (int i = 0; i < 3; i++)
        {
            var position = positions.GetAt(i);
            if (position < 0)
                return false;

            if (_data.IsBitSet(position))
            {
                _data.SetBitOff(position);
                return true;
            }
        }

        return false;
    }

    public bool UseCard(int cardId) => RemoveCard(cardId);

    public bool UseCard(Card card) => RemoveCard(card);

    public bool HasCard(Card card)
        => _data.Contains(_cardReference.ResolveId(card.Id));

    public bool HasCard(CardAction cardAction)
        => _data.Contains(_cardReference.ResolveAction(cardAction));

    private void ForEachCard(Action<Card> cardAction)
    {
        for (int i = 0; i < 128; i++)
        {
            if (_data.IsBitSet((byte)i))
            {
                cardAction.Invoke(_cardReference.GetCard(i));
            }
        }
    }

    public override string ToString()
    {
        var list = new List<string>();
        ForEachCard(c => list.Add(c.Title));
        return "{ " + string.Join(", ", list) + " }";
    }

    public CardSet DrawCards(int cardsToDraw)
    {
        var hand = new CardSet(_cardReference);
        var deckCount = Count;
        var currentIndex = 0;

        for (int i = 0; i < 128 && hand.Count < cardsToDraw; i++)
        {
            if (!_data.IsBitSet((byte)i))
                continue;

            var probability = ((double)(cardsToDraw - hand.Count)) / (deckCount - currentIndex);
            var isSelected = rng.NextDouble() <= probability;

            if (isSelected)
            {
                _data.SetBitOff((byte)i);
                hand._data.SetBitOn((byte)i);

                if (hand.Count == cardsToDraw)
                    return hand;
            }

            currentIndex++;
        }

        throw new Exception("Failed to draw cards!");
    }

    public Card DrawCard(bool random = true)
    {
        var currentIndex = 0;
        var deckCount = Count;

        for (int i = 0; i < 128; i++)
        {
            if (!_data.IsBitSet((byte)i))
                continue;

            var probability = (5.0 - 1) / (deckCount - currentIndex);
            var isSelected = !random || rng.NextDouble() <= probability;

            if (isSelected)
            {
                _data.SetBitOff((byte)i);
                return _cardReference.GetCard(i);
            }

            currentIndex++;
        }

        throw new Exception("Failed to draw a card!");
    }
}
