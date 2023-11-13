
namespace YdkTester;

public struct CardSetArray
{
    private int _bytePositions;

    public void Add(int position)
    {
        for (int i = 0; i < 24; i += 8)
        {
            if ((_bytePositions & (255 << i)) == 0)
            {
                _bytePositions |= (position + 1) << i;
                return;
            }
        }
    }

    public readonly int GetAt(int index) => ((_bytePositions >> (index * 8)) & 255) - 1;
}
