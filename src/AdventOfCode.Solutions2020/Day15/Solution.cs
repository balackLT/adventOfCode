using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day15
{
    public class Solution : ISolution
    {
        public int Day { get; } = 15;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLineAsIntArray();
            
            Debug.Assert(NthNumberSpoken(new List<int>{0,3,6}, 2020) == 436);
            
            var result = NthNumberSpoken(lines, 2020);
            
            return result.ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLineAsIntArray();
            
            // Debug.Assert(NthNumberSpoken(new List<int>{0,3,6}, 30000000) == 175594);

            var result = NthNumberSpoken(lines, 30000000);
            
            return result.ToString();
        }

        private long NthNumberSpoken(IReadOnlyList<int> numbers, long n)
        {
            var memory = new Dictionary<long, List<long>>();

            // Initialize starting numbers
            for (int i = 0; i < numbers.Count; i++)
            {
                memory[numbers[i]] = new List<long>{i};
            }
            
            // Play
            long prevNumber = numbers[^1];
            
            for (int i = numbers.Count; i < n; i++)
            {
                long number = 0;
                
                if (memory.TryGetValue(prevNumber, out var position) && position.Count > 1)
                {
                    number = i - 1 - position[^2];
                }
                
                prevNumber = number;
                if (!memory.ContainsKey(number))
                {
                    memory[number] = new List<long>();
                }
                memory[number].Add(i);
            }

            return prevNumber;
        }
    }
}