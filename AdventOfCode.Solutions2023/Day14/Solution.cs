using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;
using MoreLinq.Extensions;

namespace AdventOfCode.Solutions2023.Day14;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();
        
        MoveAllRocks(map, Coordinate.South);
        
        var result = 0L;
        var maxY = map.MaxY();
        foreach (var rock in map.Where(i => i.Value == 'O'))
        {
            result += maxY - rock.Key.Y + 1;
        }

        return result;
    }

    private static void MoveAllRocks(Dictionary<Coordinate, char> map, Coordinate direction)
    {
        var movableRocks = new Queue<Coordinate>();
        foreach (var item in map.Where(i => i.Value == 'O'))
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

        return 0;
    }
}