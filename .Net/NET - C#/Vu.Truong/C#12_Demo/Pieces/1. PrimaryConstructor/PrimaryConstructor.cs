namespace C_12_Demo.Pieces;
internal class PrimaryConstructor(int x, int y)
{
    public int Total { get; set; } = x + y;
    public int RTotal => x + y;
}

// IDE auto compile into:
internal class DecompilePrimaryConstructor
{
    private int _x;
    private int _y;
    private int _total;

    public int Total
    {
        get => _total; set => _total = value;
    }

    public int RTotal => _x + _y;

    public DecompilePrimaryConstructor(int x, int y)
    {
        _x = x;
        _y = y;
        _total = x + y;
    }
}

