
namespace YdkTester;

public struct CardSet
{
    private static Random rng = new Random();

    // Max card count is 60 + 15 + 15 = 90 (64 + 32 = 96)
    private ulong _part1;
    private uint _part2; // 6 spare bits are here, can we use them? :D

    private byte _count;
    private CardReference _cardReference; // This is a reference

    public CardSet(CardReference deckReference)
    {
        _cardReference = deckReference;
        _part1 = 0;
        _part2 = 0;
        _count = 0;
    }

    public CardSet(CardReference deckReference, byte bitstart, byte bitend)
    {
        _cardReference = deckReference;
        _part1 = 0;
        _part2 = 0;
        _count = 0;

        for (byte i = bitstart; i < bitend; i++)
        {
            SetBitOn(i);
            _count++;
        }
    }

    public CardReference CardReference => _cardReference;

    public List<Card> Cards
    {
        get
        {
            var ret = new List<Card>();

            for (byte i = 0; i < 96; i++)
            {
                if (IsBitSet(i))
                {
                    ret.Add(_cardReference.GetCard(i));
                }
            }

            return ret;
        }
    }

    public int Count => _count;

    private bool IsBitSet(byte offset)
        => (offset < 64) ?
            (_part1 & (1UL << offset)) > 0 :
            (_part2 & (1U << (offset - 64))) > 0;

    private void SetBitOn(byte offset)
    {
        if (offset < 64)
            _part1 = _part1 | (1UL << offset);
        else
            _part2 = _part2 | (1U << (offset - 64));
    }

    private void SetBitOff(byte offset)
    {
        if (offset < 64)
            _part1 = _part1 & ~(1UL << offset);
        else
            _part2 = _part2 & ~(1U << (offset - 64));
    }

    private void SetBit(byte offset, bool value)
    {
        if (value)
            SetBitOn(offset);
        else
            SetBitOff(offset);
    }

    public bool AddCard(Card card)
    {
        var positions = _cardReference.ResolveId(card.Id);

        foreach (var position in positions)
        {
            if (!IsBitSet(position))
            {
                SetBitOn(position);
                _count++;
                return true;
            }
        }

        return false;
    }

    public bool RemoveCard(Card card)
    {
        var positions = _cardReference.ResolveId(card.Id);

        foreach (var position in positions)
        {
            if (IsBitSet(position))
            {
                SetBitOff(position);
                _count--;
                return true;
            }
        }

        return false;
    }

    public bool UseCard(Card card) => RemoveCard(card);

    public bool HasCard(Card card)
        => HasCard(_cardReference.ResolveId(card.Id));

    public bool HasCard(CardAction cardAction)
        => HasCard(_cardReference.ResolveAction(cardAction));

    private bool HasCard(byte[] positions)
    {
        foreach (var position in positions)
        {
            if (IsBitSet(position))
            {
                return true;
            }
        }

        return false;
    }

    private void ForEachCard(Action<Card> cardAction)
    {
        for (int i = 0; i < 64; i++)
        {
            if ((_part1 & (1UL << i)) > 0)
            {
                cardAction.Invoke(_cardReference.GetCard(i));
            }
        }

        for (int i = 0; i < 64; i++)
        {
            if ((_part2 & (1UL << i)) > 0)
            {
                cardAction.Invoke(_cardReference.GetCard(i + 64));
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
        var deckCount = _count;

        for (int i = 0; i < deckCount && hand.Count < cardsToDraw; i++)
        {
            var probability = ((double)(cardsToDraw - hand.Count)) / (deckCount - i);
            var isSelected = rng.NextDouble() <= probability;

            if (isSelected)
            {
                SetBitOff((byte)i);
                _count--;

                hand.SetBitOn((byte)i);
                hand._count++;

                if (hand.Count == cardsToDraw)
                    return hand;
            }
        }

        return hand;
    }

    public Card DrawCard()
    {
        var deckCount = _count;

        for (int i = 0; i < deckCount; i++)
        {
            var probability = (5.0 - 1) / (deckCount - i);
            var isSelected = rng.NextDouble() <= probability;

            if (isSelected)
            {
                SetBitOff((byte)i);
                _count--;

                return _cardReference.GetCard(i);
            }
        }

        // Ugh
        throw new NotImplementedException();
    }
}
