namespace AdventOfCode.Solutions2021.Day04;

public class BingoValue
{
    public BingoValue(int value, bool isMarked = false)
    {
        this.Value = value;
        this.IsMarked = isMarked;
    }

    public int Value { get; init; }
    public bool IsMarked { get; set; }
}