using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day12;

public class Solution : ISolution
{
    public int Day { get; } = 12;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetAsCoordinateMap();

        var map = new MountainMap(lines, '~');

        var start = map.InternalMap.Single(c => c.Value == 'S').Key;
        var goal = map.InternalMap.Single(c => c.Value == 'E').Key;
        map[start] = 'z';

        var result = map.AStar(start, goal);
        
        return result.ToString();
    }
    
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetAsCoordinateMap();

        var map = new MountainMap(lines, '~');

        var goal = map.InternalMap.Single(c => c.Value == 'E').Key;

        var bestResult = int.MaxValue;
        foreach (var start in map.InternalMap.Where(c => c.Value == 'a').ToList())
        {
            var result = map.AStar(start.Key, goal);
            bestResult = Math.Min(bestResult, result);
        }
        
        return bestResult.ToString();
    }
}