using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities.Map
{
    public class Map<T>
    {
        public Dictionary<Coordinate, T> InternalMap { get; protected set; } = new();
        private T DefaultLocation { get; }

        public Map(T defaultLocation)
        {
            DefaultLocation = defaultLocation;
        }
        
        public Map(Dictionary<Coordinate, T> map, T defaultLocation)
        {
            InternalMap = map;
            DefaultLocation = defaultLocation;
        }

        public T this[Coordinate index]   
        {
            get
            {
                if (InternalMap.TryGetValue(index, out var value))
                    return value;

                InternalMap[index] = DefaultLocation;
                
                return DefaultLocation;
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
        
        public void PrintMap(Dictionary<T, char> decoder)
        {
            var minX = InternalMap.Min(m => m.Key.X);
            var maxX = InternalMap.Max(m => m.Key.X);
            var minY = InternalMap.Min(m => m.Key.Y);
            var maxY = InternalMap.Max(m => m.Key.Y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(decoder[this[new Coordinate(x, y)]]);
                }
                Console.WriteLine();
            }
        }
        
        public void PrintMapFlipY()
        {
            var minX = InternalMap.Min(m => m.Key.X);
            var maxX = InternalMap.Max(m => m.Key.X);
            var minY = InternalMap.Min(m => m.Key.Y);
            var maxY = InternalMap.Max(m => m.Key.Y);

            for (var y = maxY; y >= minY; y--)
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