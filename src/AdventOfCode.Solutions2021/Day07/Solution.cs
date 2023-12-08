using System;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2021.Day07;

public class Solution : ISolution
{
    public int Day { get; } = 7;

    public string SolveFirstPart(Input input)
    {
        var crabLocations = input
            .GetNumbersFromLines()
            .First()
            .Select(int.Parse)
            .Order()
            .ToList();

        var cheapestMove = int.MaxValue;

        foreach (int crabLocation in crabLocations.Distinct())
        {
            var cost = crabLocations.Sum(location => Math.Abs(crabLocation - location));

            cheapestMove = Math.Min(cheapestMove, cost);
        }
        
        return cheapestMove.ToString();
    }
        
    public string SolveSecondPart(Input input)
    {
        var crabLocations = input
            .GetNumbersFromLines()
            .First()
            .Select(int.Parse)
            .Order()
            .ToList();

        var cheapestMove = int.MaxValue;

        for(int i = 0; i <= crabLocations.Max(); i++)
        {
            var cost = 0;
            foreach (int location in crabLocations)
            {
                var n = Math.Abs(i - location);
                // triangle number aka 4 + 3 + 2 + 1
                cost += (n * n + n) / 2;
            }

            cheapestMove = Math.Min(cheapestMove, cost);
        }
        
        return cheapestMove.ToString();
    }
}