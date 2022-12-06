using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2021.Day04;

public class BingoBoard
{
    private readonly Dictionary<int, Coordinate> _reverseMap = new();
    public readonly Dictionary<Coordinate, BingoValue> Map;

    public BingoBoard(Map<BingoValue> map)
    {
        foreach (var i in map.InternalMap)
        {
            _reverseMap[i.Value.Value] = i.Key;
        }

        Map = map.InternalMap;
    }

    public bool IsSolved()
    {
        for (int y = 0; y < 5; y++)
        {
            var solved = Map.Where(c => c.Key.Y == y).All(b => b.Value.IsMarked);
            if (solved)
                return true;
        }
        
        for (int x = 0; x < 5; x++)
        {
            var solved = Map.Where(c => c.Key.X == x).All(b => b.Value.IsMarked);
            if (solved)
                return true;
        }

        return false;
    }
    
    public void Mark(int instruction)
    {
        if (_reverseMap.TryGetValue(instruction, out var coordinate))
            Map[coordinate].IsMarked = true;
    }
}