using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2021.Day03
{
    public class Solution : ISolution
    {
        public int Day { get; } = 3;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();

            var numbers = lines.Select(l => l.Select(c => c == '0' ? 0 : 1).ToArray()).ToList();

            var halfOfData = lines.Length / 2;
            
            var gama = new List<int>();
            var epsilon = new List<int>();
            
            for (var position = 0; position < numbers.First().Length; position++)
            {
                var counter = numbers.Count(number => number[position] > 0);

                gama.Add(counter > halfOfData ? 1 : 0);
                epsilon.Add(counter > halfOfData ? 0 : 1);
            }

            var gamaNumber = Convert.ToInt32(string.Join("", gama), 2);
            var epsilonNumber = Convert.ToInt32(string.Join("", epsilon), 2);

            return (gamaNumber * epsilonNumber).ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();

            
            return 0.ToString();
        }

    }
}