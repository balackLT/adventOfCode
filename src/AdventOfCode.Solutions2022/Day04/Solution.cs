﻿using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day04;

public class Solution : ISolution
{
    public int Day { get; } = 4;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var count = (from line in lines
            select line.Split(',')
                .Select(r => r.Split('-')
                    .Select(int.Parse)
                    .ToList())
                .ToList()
            into split
            let rangeOne = GenerateRange(split[0][0], split[0][1]).ToList()
            let rangeTwo = GenerateRange(split[1][0], split[1][1]).ToList()
            let intersect = rangeOne.Intersect(rangeTwo).ToList()
            where intersect.Count == rangeOne.Count || intersect.Count == rangeTwo.Count
            select rangeOne).Count();

        return count.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var count = (from line in lines
            select line.Split(',')
                .Select(r => r.Split('-')
                    .Select(int.Parse)
                    .ToList())
                .ToList()
            into split
            let rangeOne = GenerateRange(split[0][0], split[0][1]).ToList()
            let rangeTwo = GenerateRange(split[1][0], split[1][1]).ToList()
            let intersect = rangeOne.Intersect(rangeTwo).ToList()
            where intersect.Count > 0
            select rangeOne).Count();

        return count.ToString();
    }

    private static IEnumerable<int> GenerateRange(int from, int to)
    {
        for (int i = from; i <= to; i++)
        {
            yield return i;
        }
    }
}