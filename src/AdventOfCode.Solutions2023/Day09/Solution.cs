using System.Diagnostics;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day09;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetNumbersFromLinesAsLong();

        Debug.Assert(ExtrapolateNextNumber([0, 3, 6, 9, 12, 15]) == 18);
        Debug.Assert(ExtrapolateNextNumber([1, 3, 6, 10, 15, 21]) == 28);
        Debug.Assert(ExtrapolateNextNumber([10, 13, 16, 21, 30, 45]) == 68);

        var sum = 0L;
        foreach (List<long> sequence in lines)
        {
            sum += ExtrapolateNextNumber(sequence);
        }

        return sum.ToString();
    }
    
    private static long ExtrapolateNextNumber(IReadOnlyList<long> sequence)
    {
        if (sequence.Distinct().Count() == 1)
        {
            return sequence[0];
        }
        
        var differenceSequence = new List<long>();
        for (var i = 1; i < sequence.Count; i++)
        {
            differenceSequence.Add(sequence[i] - sequence[i - 1]);
        }
        
        return sequence[^1] + ExtrapolateNextNumber(differenceSequence);
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetNumbersFromLinesAsLong();

        Debug.Assert(ExtrapolatePreviousNumber([10, 13, 16, 21, 30, 45]) == 5);

        var sum = 0L;
        foreach (List<long> sequence in lines)
        {
            sum += ExtrapolatePreviousNumber(sequence);
        }

        return sum.ToString();
    }

    private long ExtrapolatePreviousNumber(IReadOnlyList<long> sequence)
    {
        var reversed = sequence.Reverse().ToList();
        return ExtrapolateNextNumber(reversed);
    }
}