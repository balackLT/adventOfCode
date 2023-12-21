using System.Collections.Frozen;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day21;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();
        
        var start = map.First(x => x.Value == 'S').Key;
        map[start] = '.';

        var frozenMap = map.ToFrozenDictionary();
        
        var active = new HashSet<Coordinate> {start};

        for (int i = 1; i <= 64; i++)
        {
            var newActive = new HashSet<Coordinate>();
            foreach (Coordinate coordinate in active)
            {
                var neighbours = coordinate
                    .GetAdjacent()
                    .Where(x => frozenMap.TryGetValue(x, out var value) && value == '.');

                foreach (var neighbour in neighbours)
                {
                    newActive.Add(neighbour);
                }
            }
            active = newActive;
        }
        
        return active.Count;
    }
    
    public object SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateMap();
        
        var start = map.First(x => x.Value == 'S').Key;
        map[start] = '.';
        
        var infiniteMap = new TiledMap(map);
        
        var result = infiniteMap.ReachableInXSteps(start, 500);
        // var minX = result.Min(x => x.X);
        // var maxX = result.Max(x => x.X);
        // var minY = result.Min(x => x.Y);
        // var maxY = result.Max(x => x.Y);
        // for (var x = minX; x <= maxX; x++)
        // {
        //     for (var y = minY; y <= maxY; y++)
        //     {
        //         var coordinate = new Coordinate(x, y);
        //         Console.Write(result.Contains(coordinate) ? 'O' : infiniteMap[coordinate]);
        //     }
        //     Console.WriteLine();
        // }

        return result.Count;
    }
}