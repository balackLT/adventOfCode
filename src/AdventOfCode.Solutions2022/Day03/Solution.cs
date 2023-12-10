using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day03;

public class Solution : ISolution
{
    public int Day { get; } = 3;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var totalPriority = 0;
        
        foreach (string line in lines)
        {
            var half = line.Length / 2;
            var firstHalf = line.Take(half).ToList();
            var secondHalf = line.Skip(half).Take(half).ToList();
            var match = firstHalf.Intersect(secondHalf).Single();

            var priority = (int)match;
            if (char.IsLower(match))
                priority -= 96;
            else
                priority -= 38;

            totalPriority += priority;
        }

        return totalPriority.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var totalPriority = 0;
        
        for (int i = 0; i < lines.Length; i+= 3)
        {
            var groups = lines.Skip(i).Take(3).ToList();
            var match = groups[0].Intersect(groups[1]).Intersect(groups[2]).Single();
            
            var priority = (int)match;
            if (char.IsLower(match))
                priority -= 96;
            else
                priority -= 38;

            totalPriority += priority;
        }

        return totalPriority.ToString();
    }

}