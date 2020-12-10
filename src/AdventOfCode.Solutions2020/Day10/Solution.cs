using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day10
{
    public class Solution : ISolution
    {
        public int Day { get; } = 10;

        public string SolveFirstPart(Input input)
        {
            var adapters = input
                .GetLines()
                .Select(int.Parse)
                .OrderBy(a => a)
                .ToList();

            var joltageDifference1 = 0;
            var joltageDifference3 = 1;
            var currentJoltage = 0;
            
            foreach (var adapter in adapters)
            {
                var diff = adapter - currentJoltage;
                if (diff == 1)
                    joltageDifference1++;
                else
                    joltageDifference3++;

                currentJoltage = adapter;
            }
            
            return (joltageDifference1 * joltageDifference3).ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var adapters = input
                .GetLines()
                .Select(int.Parse)
                .OrderBy(a => a)
                .ToList();

            var result = CountPaths(adapters, 0, adapters.Max());
            
            return result.ToString();
        }

        private readonly Dictionary<string, long> _cache = new();
        
        private long CountPaths(List<int> adapterList, int startingJoltage, int targetJoltage)
        {
            long result = 0;

            var key = string.Join(",", result) + $",{startingJoltage}";
            if (_cache.TryGetValue(key, out result))
                return result;

            var possiblePaths = adapterList.Where(a => a - startingJoltage <= 3);
            foreach (var path in possiblePaths)
            {
                if (path == targetJoltage)
                    result++;
                else
                    result += CountPaths(adapterList.Where(a => a > path).ToList(), path, targetJoltage);
            }
            
            _cache.Add(key, result);

            return result;
        }
    }
}