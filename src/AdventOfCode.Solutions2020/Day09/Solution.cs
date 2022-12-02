using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day09;

public class Solution : ISolution
{
    public int Day { get; } = 9;

    public string SolveFirstPart(Input input)
    {
        var codes = input
            .GetLinesAsList()
            .Select(long.Parse)
            .ToList();

        Debug.Assert(HasSum(40, new List<long>{35, 20, 15, 25, 47}));
        Debug.Assert(!HasSum(127, new List<long>{95, 102, 117, 150, 182}));

        var result = FindInvalidCode(25, codes);
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var codes = input
            .GetLinesAsList()
            .Select(long.Parse)
            .ToList();

        var invalidCode = FindInvalidCode(25, codes);

        for (int i = 0; i < codes.Count - 1; i++)
        {
            long sum = 0;
            long min = long.MaxValue;
            long max = 0;
                
            for (int j = i; j < codes.Count; j++)
            {
                min = Math.Min(codes[j], min);
                max = Math.Max(codes[j], max);
                    
                sum += codes[j];
                if (sum == invalidCode)
                    return (max + min).ToString();
            }
        }
            
        return 0.ToString();
    }
        
    private long FindInvalidCode(int preambleLength, List<long> codes)
    {
        for (int i = preambleLength; i < codes.Count; i++)
        {
            var subList = codes
                .Skip(i - preambleLength)
                .Take(preambleLength)
                .ToList();
                
            if (!HasSum(codes[i], subList))
            {
                return codes[i];
            }
        }
            
        return 0;
    }

    private bool HasSum(long target, IList<long> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            for (var j = 0; j < list.Count; j++)
            {
                if (i != j && list[i] + list[j] == target)
                    return true;
            }
        }
            
        return false;
    }
}