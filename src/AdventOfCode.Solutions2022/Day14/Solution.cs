using System.Text.Json.Nodes;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;
using MoreLinq;
using MoreLinq.Extensions;

namespace AdventOfCode.Solutions2022.Day14;

public class Solution : ISolution
{
    public int Day { get; } = 14;

    public string SolveFirstPart(Input input)
    {
        var map = ParseMap(input);

        var source = new Coordinate(500, 0); 
        map[source] = '+';
        var abyssY = map.InternalMap
            .Where(c => c.Value == '#')
            .Max(c => c.Key.Y);

        while (true)
        {
            // map.PrintMap();
            // Console.ReadLine();

            var fallingSand = map.InternalMap
                .Where(m => m.Value == '~')
                .Select(m => m.Key).ToList();
            Coordinate fallingUnit = fallingSand.Count == 0 ? source : fallingSand.Single();

            var candidates = new List<Coordinate>
            {
                fallingUnit + Coordinate.North,
                fallingUnit + Coordinate.NorthWest,
                fallingUnit + Coordinate.NorthEast
            };

            var validCandidates = candidates.Where(c => map[c] == '.').ToList();
            if (validCandidates.Count == 0)
                map[fallingUnit] = 'o';
            else
            {
                map[fallingUnit] = '.';
                map[validCandidates.First()] = '~';

                if (validCandidates.First().Y > abyssY)
                    break;
            }
        }
        
        map.PrintMap();
        var result = map.InternalMap.Count(c => c.Value == 'o');
        
        return result.ToString();
    }
    
    public string SolveSecondPart(Input input)
    {
        var map = ParseMap(input);

        var source = new Coordinate(500, 0); 
        map[source] = '+';
        var abyssY = map.InternalMap
            .Where(c => c.Value == '#')
            .Max(c => c.Key.Y);
        var floorY = abyssY + 2;

        while (true)
        {
            // Console.Clear();
            // map.PrintMap();

            var fallingSand = map.InternalMap
                .Where(m => m.Value == '~')
                .Select(m => m.Key).ToList();
            Coordinate fallingUnit = fallingSand.Count == 0 ? source : fallingSand.Single();

            var candidates = new List<Coordinate>
            {
                fallingUnit + Coordinate.North,
                fallingUnit + Coordinate.NorthWest,
                fallingUnit + Coordinate.NorthEast
            };

            var validCandidates = candidates.Where(c => map[c] == '.' && c.Y < floorY).ToList();
            if (validCandidates.Count == 0 && fallingUnit == source)
            {
                map[source] = 'o';
                break;
            }
            
            if (validCandidates.Count == 0)
                    map[fallingUnit] = 'o';
            else
            {
                map[fallingUnit] = '.';
                map[validCandidates.First()] = '~';
            }
        }
        
        map.PrintMap();
        var result = map.InternalMap.Count(c => c.Value == 'o');
        
        return result.ToString();
    }
    
    private static Map<char> ParseMap(Input input)
    {
        var lines = input
            .GetLines()
            .Select(l => l.Split(" -> ")
                .Select(c => new Coordinate(c))
                .ToList())
            .ToList();

        var map = new Map<char>('.');
        foreach (var line in lines)
        {
            var pairs = line.SlidingWindows(2);

            foreach (var pair in pairs)
            {
                var coordinateLine = pair[0].CoordinatesStraightBetween(pair[1]);
                foreach (Coordinate coordinate in coordinateLine)
                {
                    map[coordinate] = '#';
                }
            }
        }

        return map;
    }
}