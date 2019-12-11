using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions2019.Shared.Map
{
    public class Map
    {
        public readonly Dictionary<Coordinate, char> InternalMap = new Dictionary<Coordinate, char>();
        private readonly char _defaultLocation;

        public Map(char defaultLocation)
        {
            _defaultLocation = defaultLocation;
        }

        public char this[Coordinate index]   
        {
            get
            {
                if (InternalMap.TryGetValue(index, out var value))
                    return value;

                InternalMap[index] = _defaultLocation;
                
                return _defaultLocation;
            }
            set => InternalMap[index] = value;
        }

        public void PrintMap()
        {
            var minX = InternalMap.Min(m => m.Key.X);
            var maxX = InternalMap.Max(m => m.Key.X);
            var minY = InternalMap.Min(m => m.Key.Y);
            var maxY = InternalMap.Max(m => m.Key.Y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(this[new Coordinate(x, y)]);
                }
                Console.WriteLine();
            }
        }
    }
}