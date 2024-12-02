namespace AdventOfCode.Solutions2023.Day02;

public class Round
{
    public int Blue { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
}

public class Game
{
    public int Id { get; set; }
    public List<Round> Rounds { get; set; } = new();
}