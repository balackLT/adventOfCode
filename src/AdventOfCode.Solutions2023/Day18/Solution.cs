using System.Globalization;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day18;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var current = Coordinate.Zero;

        List<Vector> edgeVectors = [];
        
        foreach (string line in lines)
        {
            var split = line.Split();

            Coordinate direction = split[0] switch
            {
                "L" => Coordinate.Left,
                "R" => Coordinate.Right,
                "U" => Coordinate.Up,
                "D" => Coordinate.Down,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            int steps = int.Parse(split[1]);
            var next = current + direction * steps;
            var vector = new Vector(current, next);
            edgeVectors.Add(vector);
            
            current = next;
        }

        var result = VectorEnclosedArea(edgeVectors);
        
        return result;
    }

    private record Vector(Coordinate Start, Coordinate End);

    private static long VectorEnclosedArea(List<Vector> vectors)
    {
        long perimeter = vectors.Sum(v => v.Start.ManhattanDistance(v.End));
        
        // shoelace formula
        long interiorArea = 0;
        foreach (Vector vector in vectors)
        {
            interiorArea += vector.Start.X * vector.End.Y - vector.Start.Y * vector.End.X;
        }

        interiorArea /= 2;
        interiorArea = Math.Abs(interiorArea);

        // picks theorem
        var totalVolume = interiorArea + perimeter / 2 + 1;

        return totalVolume;
    }
    
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var current = Coordinate.Zero;

        List<Vector> edgeVectors = [];
        
        foreach (string line in lines)
        {
            var split = line.Split();

            var hexSteps = split[2].Substring(2, 5);
            var hexDirection = split[2].Substring(7, 1);
            
            //0 means R, 1 means D, 2 means L, and 3 means U.
            Coordinate direction = hexDirection switch
            {
                "2" => Coordinate.Left,
                "0" => Coordinate.Right,
                "3" => Coordinate.Up,
                "1" => Coordinate.Down,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var steps = long.Parse(hexSteps, NumberStyles.HexNumber);
            var next = current + direction * steps;
            var vector = new Vector(current, next);
            edgeVectors.Add(vector);
            
            current = next;
        }

        var result = VectorEnclosedArea(edgeVectors);
        
        return result;
    }
}