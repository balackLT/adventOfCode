using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2021.Day06;

public class Solution : ISolution
{
    public int Day { get; } = 6;

    public object SolveFirstPart(Input input)
    {
        IEnumerable<int> list = input.GetLineAsIntArray();

        for (int i = 0; i < 80 / 7; i++)
        {
            list = ProcessFish7Iterations(list.ToList());
        }
        
        

        return list.Count().ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        IEnumerable<int> list = input.GetLineAsIntArray();

        for (int i = 0; i < 256; i++)
        {
            list = ProcessFish(list);
        }

        return list.Count().ToString();
    }

    private static IEnumerable<int> ProcessFish7Iterations(List<int> fishList)
    {
        var newFish = new List<int>();

        foreach (int fish in fishList)
        {
            var nextFish = fish switch
            {
                0 => 2,
                1 => 3,
                2 => 4,
                3 => 5,
                4 => 6,
                5 => 7,
                6 => 8,
                7 => 0,
                8 => 1,
                _ => throw new ArgumentOutOfRangeException()
            };
            newFish.Add(nextFish);
            yield return fish;
        }

        foreach (var i in newFish)
        {
            yield return i;
        }
    }
    
    private static IEnumerable<int> ProcessFish(IEnumerable<int> fishList)
    {
        var append = 0;
        
        foreach (int fish in fishList)
        {
            if (fish == 0)
            {
                append++;
                yield return 6;
            }
            else
            {
                yield return fish - 1;
            }
        }

        for (int i = 0; i < append; i++)
        {
            yield return 8;
        }
    }
}