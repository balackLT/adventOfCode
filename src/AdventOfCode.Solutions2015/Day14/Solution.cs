using System;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day14
{
    public class Solution : ISolution
    {
        public int Day { get; } = 14;

        public string SolveFirstPart(Input input)
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
                var distanceTraveled = (timeLimit / (deer.Rest + deer.Period)) * deer.Speed * deer.Period;
                distanceTraveled += Math.Min((timeLimit % (deer.Rest + deer.Period)), deer.Period) * deer.Speed;
                result = Math.Max(result, distanceTraveled);
            }
            
            return result.ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLinesByRegex(@"^(\w+).+ (\d+) .+ (\d+) .+ (\d+)");

            var contestants = lines
                .Select(l => new Reindeer(
                    l[1], 
                    int.Parse(l[2]),
                    int.Parse(l[3]),
                    int.Parse(l[4])));

            for (int s = 0; s < 2503; s++)
            {
                
            }

            return 0.ToString();
        }

        private record Reindeer(string Name, int Speed, int Period, int Rest);
    }
}