using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Utilities.Extensions;

public static class CoordinateDictionaryExtensions
{
    public static Dictionary<Coordinate, char> ToMap(IEnumerable<string> inputLines)
    {
        var map = new Dictionary<Coordinate, char>();
            
        var y = 0;
        foreach (var lines in inputLines)
        {
            var x = 0;
            foreach (var pixel in lines)
            {
                var coordinate = new Coordinate(x, y);
                map[coordinate] = pixel;
                    
                x++;
            }
            y++;
        }

        return map;
    }
    
    public static int MaxX<T>(this IDictionary<Coordinate, T> map) => map.Max(c => c.Key.X);
    public static int MaxY<T>(this IDictionary<Coordinate, T> map) => map.Max(c => c.Key.Y);
    public static int MinX<T>(this IDictionary<Coordinate, T> map) => map.Min(c => c.Key.X);
    public static int MinY<T>(this IDictionary<Coordinate, T> map) => map.Min(c => c.Key.Y);
    
    public static void Print(this Dictionary<Coordinate, char> map)
    {
        var minX = map.Min(m => m.Key.X);
        var maxX = map.Max(m => m.Key.X);
        var minY = map.Min(m => m.Key.Y);
        var maxY = map.Max(m => m.Key.Y);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Console.Write(map[new Coordinate(x, y)]);
            }
            Console.WriteLine();
        }
    }

    // NB: only for a square matrix
    public static Dictionary<Coordinate, T> Rotate<T>(this Dictionary<Coordinate, T> map)
    {
        var newMap = new Dictionary<Coordinate, T>();
        var max = map.Max(c => c.Key.X);

        for (int y = 0; y <= max; y++)
        {
            for (int x = 0; x <= max; x++)
            {
                newMap[new Coordinate(x, y)] = map[new Coordinate(max - y, x)];
            }
        }

        return newMap;
    }
        
    public static Dictionary<Coordinate, T> FlipY<T>(this Dictionary<Coordinate, T> map)
    {
        var newMap = new Dictionary<Coordinate, T>();
        var maxX = map.Max(c => c.Key.X);
        var maxY = map.Max(c => c.Key.Y);
            
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                newMap[new Coordinate(x, y)] = map[new Coordinate(maxX -x, y)];
            }
        }

        return newMap;
    }
        
    public static Dictionary<Coordinate, T> FlipX<T>(this Dictionary<Coordinate, T> map)
    {
        var newMap = new Dictionary<Coordinate, T>();
        var maxX = map.Max(c => c.Key.X);
        var maxY = map.Max(c => c.Key.Y);
            
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++)
            {
                newMap[new Coordinate(x, y)] = map[new Coordinate(x, maxY - y)];
            }
        }

        return newMap;
    }
}