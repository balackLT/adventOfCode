using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day13;

public class Solution : ISolution
{
    public int Day { get; } = 13;
        
    public object SolveFirstPart(Input input)
    {
        var program = input.GetLineAsLongArray();

        var arcade = new Arcade(program);
        arcade.RunDemo();
        var result = arcade.CountTiles(2);
            
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var program = input.GetLineAsLongArray();

        var arcade = new Arcade(program);
        arcade.Run();
            
        return arcade.Score.ToString();
    }
}