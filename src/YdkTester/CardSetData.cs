
namespace YdkTester;

public struct CardSetData
{
    // Max card count is 60 + 15 + 15 = 90 (64 + 32 = 96)
    private ulong _part1;
    private uint _part2; // 6 spare bits are here, can we use them? :D
    private byte _count;

    public readonly int Count => _count;

    public readonly bool IsBitSet(int offset)
        => (offset < 64) ?
            (_part1 & (1UL << offset)) > 0 :
            (_part2 & (1U << (offset - 64))) > 0;

    public void SetBitOn(int offset)
    {
        if (!IsBitSet(offset))
            _count++;

        if (offset < 64)
            _part1 |= 1UL << offset;
        else
            _part2 |= 1U << (offset - 64);
    }

    public void SetBitOff(int offset)
    {
        if (IsBitSet(offset))
            _count--;

        if (offset < 64)
            _part1 &= ~(1UL << offset);
        else
            _part2 &= ~(1U << (offset - 64));
    }

    public void SetBit(int offset, bool value)
    {
        if (value)
            SetBitOn(offset);
        else
            SetBitOff(offset);
    }

    public readonly bool Contains(CardSetData data) => ((_part1 & data._part1) | (_part2 & data._part2)) > 0;
}
