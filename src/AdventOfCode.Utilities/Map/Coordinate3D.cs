using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities.Map;

public record Coordinate3D (long X, long Y, long Z)
{
    public Coordinate3D(string input, char separator = ',') : this(
        long.Parse(input.Split(separator)[0]),
        long.Parse(input.Split(separator)[1]),
        long.Parse(input.Split(separator)[2]))
    {
            
    }
    
    public static Coordinate3D operator +(Coordinate3D left, Coordinate3D right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }
    
    public long ManhattanDistance(Coordinate3D other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
    }
    
    public Coordinate3D MoveTowards(Coordinate3D coordinate)
    {
        var adjacent = GetAdjacent();
        return adjacent.MinBy(coordinate.ManhattanDistance);
    }
    
    public IEnumerable<Coordinate3D> CoordinatesStraightBetween(Coordinate3D target)
    {
        var current = this;
        while (current != target)
        {
            yield return current;
            current = current.MoveTowards(target);
        }
        yield return current;
    }
        
    public IEnumerable<Coordinate3D> GetAdjacent()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (!(x == 0 && y == 0 && z == 0))
                        yield return new Coordinate3D(x, y, z) + this;
                }
            }
        }
    }
}