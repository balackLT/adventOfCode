using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions2020.Day17
{
    public class Map4D
    {
        public Dictionary<Coordinate4D, bool> InternalMap { get; } = new();

        public int MaxX { get; private set; }
        public int MinX { get; private set; }
        public int MaxY { get; private set; }
        public int MinY { get; private set; }
        public int MaxZ { get; private set; }
        public int MinZ { get; private set; }
        public int MaxW { get; private set; }
        public int MinW { get; private set; }
        
        
        public bool this[Coordinate4D index]
        {
            get
            {
                if (InternalMap.TryGetValue(index, out var value))
                    return value;

                InternalMap[index] = false;
            
                return false;
            }
            set
            {
                InternalMap[index] = value;
                MaxX = Math.Max(MaxX, index.X);
                MinX = Math.Min(MinX, index.X);
                MaxY = Math.Max(MaxY, index.Y);
                MinY = Math.Min(MinY, index.Y);
                MaxZ = Math.Max(MaxZ, index.Z);
                MinZ = Math.Min(MinZ, index.Z);
                MaxW = Math.Max(MaxW, index.W);
                MinW = Math.Min(MinW, index.W);
            }
        }

        public void DoConway4D()
        {
            var coordinatesToFlip = new List<Coordinate4D>();

            for (int x = MinX - 1; x <= MaxX + 1; x++)
            {
                for (int y = MinY - 1; y <= MaxY + 1; y++)
                {
                    for (int z = MinZ - 1; z <= MaxZ + 1; z++)
                    {
                        for (int w = MinZ - 1; w <= MaxZ + 1; w++)
                        {
                            var coordinate = new Coordinate4D(x, y, z, w);
                            var neighbours = coordinate.GetAdjacent();
                            var activeFriends = neighbours.Where(n => this[n]);
                            var active = activeFriends.Count();

                            if (this[coordinate] && !(active == 2 || active == 3))
                                coordinatesToFlip.Add(coordinate);
                            else if (this[coordinate] == false && active == 3)
                                coordinatesToFlip.Add(coordinate);
                        }
                    }
                }
            }

            foreach (var coord in coordinatesToFlip)
            {
                this[coord] = !this[coord];
            }
        }
    }
}