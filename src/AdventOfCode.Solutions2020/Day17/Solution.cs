using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day17;

public class Solution : ISolution
{
    public int Day { get; } = 17;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetAsCoordinateMap();

        var map = new Map3D();
            
        foreach ((Coordinate coordinate2D, var value) in lines)
        {
            var coordinate = new Coordinate3D(coordinate2D.X, coordinate2D.Y, 0);
            map[coordinate] = (value == '#');
        }
            
        for (int i = 0; i < 6; i++)
        {
            map.DoConway3D();
        }

        var result = map.InternalMap.Count(c => c.Value);
            
        return result.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetAsCoordinateMap();

        var map = new Map4D();
            
        foreach ((Coordinate coordinate2D, var value) in lines)
        {
            var coordinate = new Coordinate4D(coordinate2D.X, coordinate2D.Y, 0, 0);
            map[coordinate] = (value == '#');
        }
            
        for (int i = 0; i < 6; i++)
        {
            map.DoConway4D();
        }

        var result = map.InternalMap.Count(c => c.Value);
            
        return result.ToString();
    }
}