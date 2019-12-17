using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day14
{
    public struct Material
    {
        public readonly string Name;
        public int Quantity;

        public Material(string source)
        {
            var elements = source.Trim().Split(' ');
            Quantity = int.Parse(elements[0]);
            Name = elements[1].Trim();
        }
    }
    
    public class Solution : ISolution
    {
        public int Day { get; } = 14;
        
        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();
            var formulas = lines
                .Select(l => l.Split("=>"))
                .Select(l => (Source: l[0].Split(','), Result: l[1]))
                .Select(m => (Source: m.Source.Select(f => new Material(f)), Result: new Material(m.Result)))
                .ToList();
            
            var result = GetRequiredOre(formulas, 1, "FUEL", new Dictionary<string, long>());
            
            return result.ToString();
        }

        private long GetRequiredOre(List<(IEnumerable<Material> Source, Material Result)> formulas, long count, string target, Dictionary<string, long> extraStuff)
        {
            var formula = formulas.First(f => f.Result.Name == target);

            long result = 0;

            foreach (var source in formula.Source)
            {
                var material = source;
                var requiredAmount = material.Quantity * count;

                if (source.Name == "ORE")
                {
                    result += requiredAmount;
                    continue;
                }

                if (extraStuff.TryGetValue(material.Name, out var amount) && amount > 0)
                {
                    if (amount >= requiredAmount)
                    {
                        extraStuff[material.Name] -= requiredAmount;
                        continue;
                    }

                    requiredAmount -= extraStuff[material.Name];
                    extraStuff[material.Name] = 0;
                }

                var produced = formulas.First(f => f.Result.Name == source.Name).Result.Quantity;
                var batches = requiredAmount / produced;
                if (requiredAmount % produced != 0)
                    batches++;

                var leftover = (produced * batches) - requiredAmount;
                Debug.Assert(leftover >= 0);
                
                if (extraStuff.ContainsKey(material.Name))
                    extraStuff[material.Name] += leftover;
                else extraStuff[material.Name] = leftover;

                result += GetRequiredOre(formulas, batches, material.Name, extraStuff);
            }
            
            return result;
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();
            var formulas = lines
                .Select(l => l.Split("=>"))
                .Select(l => (Source: l[0].Split(','), Result: l[1]))
                .Select(m => (Source: m.Source.Select(f => new Material(f)), Result: new Material(m.Result)))
                .ToList();
            
            var requiredForOne = GetRequiredOre(formulas, 1, "FUEL", new Dictionary<string, long>());

            const long target = 1000000000000;
            var lowerBound = target / requiredForOne;
            var upperBound = lowerBound * 10;

            var result = BinarySearch(formulas, lowerBound, upperBound, target);
            
            return result.ToString();
        }

        public long BinarySearch(List<(IEnumerable<Material> Source, Material Result)> formulas, long lower, long upper, long target)
        {
            if (upper - lower <= 1)
                return lower;
            
            var average = (lower + upper) / 2 ;
            
            var result = GetRequiredOre(formulas, average, "FUEL", new Dictionary<string, long>());
            // Console.WriteLine($"Fuel: {average}, Result: {result}");
            
            if (result > target)
                return BinarySearch(formulas, lower, average, target);
            if (result < target)
                return BinarySearch(formulas, average, upper, target);
            
            return result;
        }
    }
}