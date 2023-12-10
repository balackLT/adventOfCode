using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day14;

public class Solution : ISolution
{
    public int Day { get; } = 14;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLinesByRegex(@"^(\w+).+ (\d+) .+ (\d+) .+ (\d+)");

        var contestants = lines
            .Select(l => new Reindeer(
                l[1], 
                int.Parse(l[2]),
                int.Parse(l[3]),
                int.Parse(l[4])));

        var result = 0;
        var timeLimit = 2503;
            
        foreach (var deer in contestants)
        {
            var distanceTraveled = deer.CalculateDistanceTraveled(timeLimit);
            result = Math.Max(result, distanceTraveled);
        }
            
        return result.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLinesByRegex(@"^(\w+).+ (\d+) .+ (\d+) .+ (\d+)");

        var contestants = lines
            .Select(l => new Reindeer(
                l[1], 
                int.Parse(l[2]),
                int.Parse(l[3]),
                int.Parse(l[4])))
            .ToList();

        var points = new Dictionary<Reindeer, int>();
        foreach (var reindeer in contestants)
        {
            points[reindeer] = 0;
        }

        var limit = 2503;
            
        for (int s = 1; s <= limit; s++)
        {
            var maxDistance = contestants.Max(c => c.CalculateDistanceTraveled(s));
            var leaders = contestants.Where(c => c.CalculateDistanceTraveled(s) == maxDistance).ToList();
            foreach (var leader in leaders)
            {
                points[leader]++;
            }
        }

        var result = points.Max(p => p.Value);

        return result.ToString();
    }

    private record Reindeer(string Name, int Speed, int Period, int Rest)
    {
        public int CalculateDistanceTraveled(int time)
        {
            var distanceTraveled = (time / (Rest + Period)) * Speed * Period;
            return distanceTraveled + Math.Min((time % (Rest + Period)), Period) * Speed;
        }
    }
}