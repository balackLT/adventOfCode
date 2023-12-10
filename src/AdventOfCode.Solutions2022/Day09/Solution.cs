using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2022.Day09;

public class Solution : ISolution
{
    public int Day { get; } = 9;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var head = Coordinate.Zero;
        var tail = Coordinate.Zero;
        var visited = new List<Coordinate>{tail};

        foreach (var instruction in lines.Select(l => l.Split()))
        {
            var direction = instruction[0] switch
            {
                "D" => Coordinate.South,
                "U" => Coordinate.North,
                "R" => Coordinate.East,
                "L" => Coordinate.West,
                _ => throw new ArgumentOutOfRangeException()
            };

            var distance = int.Parse(instruction[1]);

            for (int i = 0; i < distance; i++)
            {
                head += direction;

                if (head.IsAdjacentWithDiagonals(tail) || head == tail)
                {
                    // noop
                }
                else
                {
                    var target = tail.MoveTowardsWithDiagonals(head);
                    tail = target;
                    visited.Add(tail);
                }
            }
        }
        
        return visited.Distinct().Count().ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var head = Coordinate.Zero;

        var tails = Enumerable.Repeat(Coordinate.Zero, 9).ToList();
        var visited = new List<Coordinate>{Coordinate.Zero};

        foreach (var instruction in lines.Select(l => l.Split()))
        {
            var direction = instruction[0] switch
            {
                "D" => Coordinate.South,
                "U" => Coordinate.North,
                "R" => Coordinate.East,
                "L" => Coordinate.West,
                _ => throw new ArgumentOutOfRangeException()
            };

            var distance = int.Parse(instruction[1]);

            for (int i = 0; i < distance; i++)
            {
                head += direction;

                var localHead = head;
                for (int t = 0; t < 9; t++)
                {
                    var tail = tails[t];
                    if (localHead!.IsAdjacentWithDiagonals(tail) || localHead == tail)
                    {
                        // noop
                    }
                    else
                    {
                        var target = tail.MoveTowardsWithDiagonals(localHead);
                        tail = target;

                        if (t == 8)
                        {
                            visited.Add(tail!);
                        }
                    }

                    tails[t] = tail!;
                    localHead = tail;
                }
            }
        }
        
        return visited.Distinct().Count().ToString();
    }
}