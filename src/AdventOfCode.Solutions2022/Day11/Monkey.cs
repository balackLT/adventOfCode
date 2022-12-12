namespace AdventOfCode.Solutions2022.Day11;

public class Monkey
{
    public Queue<long> Items { get; set; } = new();
    public Operation Operation { get; set; }
    public int OperationAmount { get; set; } = -1;
    public int Test { get; set; } = 0;
    public int True { get; set; } = 0;
    public int False { get; set; } = 0;
    public long MonkeyBusiness { get; set; } = 0;
}

public enum Operation
{
    MULTIPLY,
    SQUARE,
    ADD
}