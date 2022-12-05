using System.Text.RegularExpressions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day05;

public class Solution : ISolution
{
    public int Day { get; } = 5;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();
        
        var towers = ParseTowers(lines);

        var instructionRegex = new Regex(@"\d+", RegexOptions.Compiled);

        foreach (var line in lines.Where(l => l.StartsWith("move")))
        {
            var numbers = instructionRegex.Matches(line).Select(m => int.Parse(m.Value)).ToList();

            for (int i = 0; i < numbers[0]; i++)
            {
                var crate = towers[numbers[1] - 1].Pop();
                towers[numbers[2] - 1].Push(crate);
            }
        }

        var result = towers.Select(t => t.Pop());
        
        return string.Concat(result);
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();
        
        var towers = ParseTowers(lines);

        var instructionRegex = new Regex(@"\d+", RegexOptions.Compiled);

        foreach (var line in lines.Where(l => l.StartsWith("move")))
        {
            var numbers = instructionRegex.Matches(line).Select(m => int.Parse(m.Value)).ToList();

            var stack = new Stack<char>();
            
            for (int i = 0; i < numbers[0]; i++)
            {
                stack.Push(towers[numbers[1] - 1].Pop());
            }

            while (stack.TryPop(out var crate))
            {
                towers[numbers[2] - 1].Push(crate);
            }
        }

        var result = towers.Select(t => t.Pop());
        
        return string.Concat(result);
    }

    private static List<Stack<char>> ParseTowers(IEnumerable<string> lines)
    {
        var towers = new List<Stack<char>>();

        foreach (string line in lines.Where(l => l.StartsWith('[')).Reverse())
        {
            for (int i = 0; i < line.Skip(1).Count(); i += 4)
            {
                if (towers.Count < i / 4 + 1)
                {
                    towers.Add(new Stack<char>());
                }

                var character = line[i + 1];
                if (char.IsLetter(character))
                {
                    towers[i / 4].Push(character);
                }
            }
        }

        return towers;
    }
}