using AdventOfCode.Abstractions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day01;

public class Solution : SolutionBase
{
    public override int Year => 2022;
    public override int Day => 1;

    public override string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var elves = new List<int>();
        var elf = 0;

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(elf);
                elf = 0;
            }
            else
            {
                elf += int.Parse(line);
            }
        }
            
        return elves.Max().ToString();
    }

    public override string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var elves = new List<int>();
        var elf = 0;

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                elves.Add(elf);
                elf = 0;
            }
            else
            {
                elf += int.Parse(line);
            }
        }

        return elves.OrderDescending().Take(3).Sum().ToString();
    }

}