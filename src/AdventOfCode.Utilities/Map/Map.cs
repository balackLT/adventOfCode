using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities.Map;

public class Map<T> 
{
    public Dictionary<Coordinate, T> InternalMap { get; protected set; } = new();
    
    public int MinX => InternalMap.Min(m => m.Key.X);
    public int MaxX => InternalMap.Max(m => m.Key.X);
    public int MinY => InternalMap.Min(m => m.Key.Y);
    public int MaxY => InternalMap.Max(m => m.Key.Y);

    public long ApproximateSize => (MaxX - MinX) * (MaxY - MinY); 
    
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
        for (var y = MinY; y <= MaxY; y++)
        {
            //Console.Write($"{y:000} ");
            for (var x = MinX; x <= MaxX; x++)
            {
                Console.Write(this[new Coordinate(x, y)]);
            }
            Console.WriteLine();
        }
    }
        
    public void PrintMap(Dictionary<T, char> decoder)
    {
        for (var y = MinY; y <= MaxY; y++)
        {
            for (var x = MinX; x <= MaxX; x++)
            {
                Console.Write(decoder[this[new Coordinate(x, y)]]);
            }
            Console.WriteLine();
        }
    }
        
    public void PrintMapFlipY()
    {
        for (var y = MaxY; y >= MinY; y--)
        {
            for (var x = MinX; x <= MaxX; x++)
            {
                Console.Write(this[new Coordinate(x, y)]);
            }
            Console.WriteLine();
        }
    }
}