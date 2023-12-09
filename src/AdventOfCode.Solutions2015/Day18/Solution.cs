using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2015.Day18;

public class Solution : ISolution
{
    public string SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        for (int i = 0; i < 100; i++)
        {
            var newMap = new Dictionary<Coordinate, char>();
            foreach (var coordinate in map)
            {
                var neighbours = coordinate.Key.GetAdjacentWithDiagonals();
                var onNeighbours = neighbours.Count(n => map.ContainsKey(n) && map[n] == '#');
                
                if (coordinate.Value == '#')
                {
                    newMap[coordinate.Key] = onNeighbours is 2 or 3 ? '#' : '.';
                }
                else
                {
                    newMap[coordinate.Key] = onNeighbours == 3 ? '#' : '.';
                }
            }
            map = newMap;
        }
        
        return map.Count(c => c.Value == '#').ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var alwaysOn = new List<Coordinate>
        {
            new(0, 0),
            new(0, map.MaxY()),
            new(map.MaxX(), 0),
            new(map.MaxX(), map.MaxY())
        };
        
        foreach (Coordinate coordinate in alwaysOn)
        {
            map[coordinate] = '#';
        }
        
        for (int i = 0; i < 100; i++)
        {
            var newMap = new Dictionary<Coordinate, char>();
            foreach (var coordinate in map)
            {
                var neighbours = coordinate.Key.GetAdjacentWithDiagonals();
                var onNeighbours = neighbours.Count(n => map.ContainsKey(n) && map[n] == '#');
                
                if (coordinate.Value == '#')
                {
                    newMap[coordinate.Key] = onNeighbours is 2 or 3 ? '#' : '.';
                }
                else
                {
                    newMap[coordinate.Key] = onNeighbours == 3 ? '#' : '.';
                }
            }

            foreach (Coordinate coordinate in alwaysOn)
            {
                newMap[coordinate] = '#';
            }
            
            map = newMap;
        }
        
        return map.Count(c => c.Value == '#').ToString();
    }
}