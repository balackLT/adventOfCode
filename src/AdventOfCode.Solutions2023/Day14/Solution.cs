using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day14;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();
        
        MoveAllRocks(map, Coordinate.South);
        
        long result = CalculateWeight(map);

        return result;
    }

    private static void MoveAllRocks(Dictionary<Coordinate, char> map, Coordinate direction)
    {
        var movableRocks = new Queue<Coordinate>();

        IEnumerable<KeyValuePair<Coordinate, char>> allRocks = map.Where(i => i.Value == 'O');
        IEnumerable<KeyValuePair<Coordinate, char>> orderedRocks = new List<KeyValuePair<Coordinate, char>>();
        if (direction == Coordinate.South)
        {
            orderedRocks = allRocks;
        }
        else if (direction == Coordinate.West)
        {
            orderedRocks = allRocks.OrderBy(i => i.Key.X);
        }
        else if (direction == Coordinate.North)
        {
            orderedRocks = allRocks.OrderByDescending(i => i.Key.Y);
        }
        else if (direction == Coordinate.East)
        {
            orderedRocks = allRocks.OrderByDescending(i => i.Key.X);
        }
        
        foreach (var item in orderedRocks)
        {
            movableRocks.Enqueue(item.Key);
        }

        while (movableRocks.Count != 0)
        {
            var current = movableRocks.Dequeue();
            var original = current;
            
            while (true)
            {
                var next = current + direction;
                if (map.TryGetValue(next, out char value) && value == '.')
                {
                    current = next;
                }
                else
                {
                    map[original] = '.';
                    map[current] = 'O';
                    break;
                }
            }
        }
    }

    public object SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var max = 1000000000;
        var previousMaps = new List<string>();
        var cycleSize = 0;
        for (long i = 0; i < max; i++)
        {
            var hash = map.Aggregate("", (current, c) => current + c.Value);
            if (cycleSize == 0 && previousMaps.Contains(hash))
            {
                // cycle detected, only boring stuff further on
                cycleSize = previousMaps.Count - previousMaps.LastIndexOf(hash);
                // skip boring cycles
                while (i < max - cycleSize)
                {
                    i += cycleSize;
                }
            }
            previousMaps.Add(hash);

            map = SpinCycle(map);
        }
        
        long result = CalculateWeight(map);

        return result;
    }

    private readonly Dictionary<string, Dictionary<Coordinate, char>> _cache = new();

    private Dictionary<Coordinate, char> SpinCycle(Dictionary<Coordinate, char> map)
    {
        var hash = map.Aggregate("", (current, c) => current + c.Value);
        
        if (_cache.TryGetValue(hash, out var cachedMap))
        {
            return cachedMap;
        }

        var newMap = SpinCycleCore(map);
        _cache[hash] = newMap;
        return newMap;
    }
    
    private static Dictionary<Coordinate, char> SpinCycleCore(Dictionary<Coordinate, char> map)
    {
        var newMap = new Dictionary<Coordinate, char>(map);
        
        MoveAllRocks(newMap, Coordinate.South);
        MoveAllRocks(newMap, Coordinate.West);
        MoveAllRocks(newMap, Coordinate.North);
        MoveAllRocks(newMap, Coordinate.East);

        return newMap;
    }

    private static long CalculateWeight(Dictionary<Coordinate, char> map)
    {
        var result = 0L;
        var maxY = map.MaxY();
        foreach (var rock in map.Where(i => i.Value == 'O'))
        {
            result += maxY - rock.Key.Y + 1;
        }

        return result;
    }
}