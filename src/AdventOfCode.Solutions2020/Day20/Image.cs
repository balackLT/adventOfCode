using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day20
{
    public class Image
    {
        public Dictionary<Coordinate, char> Map { get; set; }

        public static Image Build(Dictionary<Coordinate, Tile> puzzle)
        {
            var map = new Dictionary<Coordinate, char>();

            var edge = puzzle.First().Value.InnerMap.Max(c => c.Key.X);
            
            foreach (var tile in puzzle)
            {
                foreach (var pixel in tile.Value.InnerMap)
                {
                    var coordinate = new Coordinate(pixel.Key.X + tile.Key.X * edge, pixel.Key.Y + tile.Key.Y * edge);
                    map[coordinate] = pixel.Value;
                }
            }

            return new Image
            {
                Map = map
            };
        }

        public void Print()
        {
            var minX = Map.Min(m => m.Key.X);
            var maxX = Map.Max(m => m.Key.X);
            var minY = Map.Min(m => m.Key.Y);
            var maxY = Map.Max(m => m.Key.Y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(Map[new Coordinate(x, y)]);
                }
                Console.WriteLine();
            }
        }
    }
}