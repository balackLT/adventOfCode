using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Solutions2022.Day06;

public class Solution : ISolution
{
    public int Day { get; } = 6;

    public object SolveFirstPart(Input input)
    {
        var line = input.GetAsString();

        var marker = line
            .SlidingWindows(4)
            .First(w => w.Distinct().Count() == 4)
            .ConcatToString();

        var location = line.IndexOf(marker, StringComparison.InvariantCulture) + 4;
        
        return location.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var line = input.GetAsString();

        var marker = line
            .SlidingWindows(14)
            .First(w => w.Distinct().Count() == 14)
            .ConcatToString();

        var location = line.IndexOf(marker, StringComparison.InvariantCulture) + 14;
        
        return location.ToString();
    }
}