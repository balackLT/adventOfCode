using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day16;

public class Solution : ISolution
{
    public int Day { get; } = 16;
        
    public string SolveFirstPart(Input input)
    {
        var signal = input.GetAsString().Select(i => int.Parse(i.ToString())).ToList();
        var pattern = new List<int> {0, 1, 0, -1};

        for (var i = 0; i < 100; i++)
        {
            signal = NaiveFFT(signal, pattern);
        }

        var result = string.Concat(signal.Take(8));
            
        return result;
    }

    public List<int> NaiveFFT(List<int> signal, List<int> pattern)
    {
        var result = new List<int>();
            
        for (var i = 0; i < signal.Count(); i++)
        {
            var newSignal = 0;
            var tempPattern = pattern
                .Select(p => Enumerable.Repeat(p, i + 1))
                .SelectMany(i => i)
                .Skip(1)
                .ToList();

            var tempTemp = pattern
                .Select(p => Enumerable.Repeat(p, i + 1))
                .SelectMany(i => i)
                .ToList();

            var j = 0;
            foreach (var s in signal)
            {
                if (tempPattern.Count <= signal.Count)
                    tempPattern.AddRange(tempTemp);
                    
                newSignal += s * tempPattern[j];
                j++;
            }
                
            result.Add(Math.Abs(newSignal) % 10);
        }

        return result;
    }

    public string SolveSecondPart(Input input)
    {
        var signal = Enumerable.Repeat(input.GetAsString()
                .Select(i => int.Parse(i.ToString())), 10000)
            .SelectMany(x => x).ToList();

        var offset = int.Parse(string.Concat(signal.Take(7)));
            
        signal = signal.Skip(offset).ToList();
            
        for (var i = 0; i < 100; i++)
        {
            signal = CheapFFT(signal);
        }
            
        var result = string.Concat(signal.Take(8));
            
        return result;
    }

    private List<int> CheapFFT(List<int> signal)
    {
        var result = new int[signal.Count()];

        result[signal.Count() - 1] = signal[signal.Count() - 1];
            
        for (var i = signal.Count() - 2; i >= 0; i--)
        {
            var newSignal = (signal[i] + result[i + 1]) % 10;
            result[i] = newSignal;
        }

        return result.ToList();
    }
}