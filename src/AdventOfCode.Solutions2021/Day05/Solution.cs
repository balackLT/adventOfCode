using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2021.Day05;

public class Solution : ISolution
{
    public int Day { get; } = 5;

    public string SolveFirstPart(Input input)
    {
        var inputLines = input.GetLines();

        var regex = new Regex(@"\d+", RegexOptions.Compiled);
        var lines = inputLines
            .Select(l => regex
                .Matches(l)
                .Select(m => int.Parse(m.Value)).ToList())
            .Select(p => new List<Coordinate>
            {
                new Coordinate(p[0], p[1]),
                new Coordinate(p[2], p[3])
            });

        foreach (var line in lines.Where(l => l[0].X == l[1].X || l[0].Y == l[1].Y))
        {
            
        }

        return 0.ToString();
    }
        
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

            
        return 0.ToString();
    }

    private record Line(Coordinate From, Coordinate To);
}