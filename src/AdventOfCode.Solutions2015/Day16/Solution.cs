using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day16
{
    public class Solution : ISolution
    {
        public int Day { get; } = 16;

        public string SolveFirstPart(Input input)
        {
            var lines = input
                .GetLines()
                .Select(l => l.Split())
                .Select(l => (int.Parse(l[0].Split()[1]), l[1].Split(',')));

            

            return 0.ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();
            
            
            return 0.ToString();
        }

        private record Sue (int Number, Dictionary<string, int> Qualities);
    }
}