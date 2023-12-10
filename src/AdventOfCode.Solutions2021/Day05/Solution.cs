using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2021.Day05;

public class Solution : ISolution
{
    public int Day { get; } = 5;

    public object SolveFirstPart(Input input)
    {
        var inputLines = input.GetLines();

        var regex = new Regex(@"\d+", RegexOptions.Compiled);
        var lines = inputLines
            .Select(l => regex
                .Matches(l)
                .Select(m => int.Parse(m.Value)).ToList())
            .Select(p => new List<Coordinate>
            {
                new(p[0], p[1]),
                new(p[2], p[3])
            });

        var map = new Map<int>(0);
        
        foreach (var line in lines.Where(l => l[0].X == l[1].X || l[0].Y == l[1].Y))
        {
            var between = line[0].CoordinatesStraightBetween(line[1]);
            foreach (Coordinate coordinate in between)
            {
                map[coordinate] += 1;
            }
        }
        
        var result = map.InternalMap.Count(c => c.Value > 1);

        return result.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var inputLines = input.GetLines();

        var regex = new Regex(@"\d+", RegexOptions.Compiled);
        var lines = inputLines
            .Select(l => regex
                .Matches(l)
                .Select(m => int.Parse(m.Value)).ToList())
            .Select(p => new List<Coordinate>
            {
                new(p[0], p[1]),
                new(p[2], p[3])
            });

        var map = new Map<int>(0);
        
        foreach (var line in lines)
        {
            var between = line[0].CoordinatesBetweenWithDiagonals(line[1]);
            foreach (Coordinate coordinate in between)
            {
                map[coordinate] += 1;
            }
        }
        
        var result = map.InternalMap.Count(c => c.Value > 1);

        return result.ToString();
    }
}