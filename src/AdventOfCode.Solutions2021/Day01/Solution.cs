using System.Collections.Generic;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2021.Day01;

public class Solution : ISolution
{
    public int Day { get; } = 1;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLinesAsInt();

        var prevLine = int.MaxValue;
        var increaseCount = 0;
            
        foreach (var line in lines)
        {
            if (line > prevLine)
                increaseCount++;

            prevLine = line;
        }

        return increaseCount.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLinesAsInt();

        var sums = new List<int>();

        for (var i = 2; i < lines.Length; i++)
        {
            var slidingSum = lines[i] + lines[i - 1] + lines[i - 2];
            sums.Add(slidingSum);
        }

        var prevSum = int.MaxValue;
        var increaseCount = 0;
            
        foreach (var sum in sums)
        {
            if (sum > prevSum)
                increaseCount++;

            prevSum = sum;
        }
            
        return increaseCount.ToString();
    }

}