using System.Collections.Generic;

namespace AdventOfCode.Solutions2020.Day17
{
    public record Coordinate3D (int X, int Y, int Z)
    {
        public static Coordinate3D operator +(Coordinate3D left, Coordinate3D right)
        {
            return new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
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
}