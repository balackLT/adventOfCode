using System.Collections.Generic;

namespace AdventOfCode.Utilities.Map;

public record Coordinate4D (long X, long Y, long Z, long W)
{
    public static Coordinate4D operator +(Coordinate4D left, Coordinate4D right)
    {
        return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }
        
    public IEnumerable<Coordinate4D> GetAdjacent()
    {
        for (long x = -1; x <= 1; x++)
        {
            for (long y = -1; y <= 1; y++)
            {
                for (long z = -1; z <= 1; z++)
                {
                    for (long w = -1; w <= 1; w++)
                    {
                        if (!(x == 0 && y == 0 && z == 0 && w == 0))
                            yield return new Coordinate4D(x, y, z, w) + this;
                    }
                }
            }
        }
    }
}