using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day21;

public class TiledMap
{
    public TiledMap(IDictionary<Coordinate, char> internalMap)
    {
        InternalMap = internalMap;
        MaxX = InternalMap.Max(m => m.Key.X);
        MaxY = InternalMap.Max(m => m.Key.Y);
    }

    public IDictionary<Coordinate, char> InternalMap { get; init; }
    public long MaxX { get; init; }
    public long MaxY { get; init; }
          
    // private readonly Dictionary<string, long> _reachableCache = new();
    
    public HashSet<Coordinate> ReachableInXSteps(Coordinate current, long x)
    {
        HashSet<Coordinate> active = [current];

        long previous = 0L;
        for (int i = 1; i <= x; i++)
        {
            HashSet<Coordinate> newActive = [];
            foreach (Coordinate coordinate in active)
            {
                var neighbours = coordinate
                    .GetAdjacent()
                    .Where(c => this[c] == '.');

                foreach (var neighbour in neighbours)
                {
                    newActive.Add(neighbour);
                }
            }
            active = newActive;
            Console.WriteLine($"{i}: {active.Count}, growth: {active.Count - previous}");
            previous = active.Count;
        }
        
        return active;
    }
    
    public char this[Coordinate index]   
    {
        get
        {
            if (InternalMap.TryGetValue(index, out var value))
                return value;

            Coordinate newCoordinate = NormalizeCoordinate(index);

            return InternalMap[newCoordinate];
        }
    }

    private Coordinate NormalizeCoordinate(Coordinate index)
    {
        // if coordinate is out of bounds, it wraps around the map
        var newX = index.X;
        var newY = index.Y;
        if (index.X > MaxX || index.X < 0)
        {
            newX = (index.X % (MaxX + 1) + MaxX + 1) % (MaxX + 1);
        }
        if (index.Y > MaxY || index.Y < 0)
        {
            newY = (index.Y % (MaxY + 1) + MaxY + 1) % (MaxY + 1);
        }
            
        var newCoordinate = new Coordinate(newX, newY);
        return newCoordinate;
    }
}