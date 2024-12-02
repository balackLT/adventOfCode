using System.Collections.Concurrent;
using System.Diagnostics;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day12;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var springs = input
            .GetLines()
            .Select(l => new Spring(l))
            .ToList();

        Debug.Assert(new Spring("???.### 1,1,3").CalculatePermutations() == 1);
         Debug.Assert(new Spring(".??..??...?##. 1,1,3").CalculatePermutations() == 4);
         Debug.Assert(new Spring("?#?#?#?#?#?#?#? 1,3,1,6").CalculatePermutations() == 1);
         Debug.Assert(new Spring("????.#...#... 4,1,1").CalculatePermutations() == 1);
         Debug.Assert(new Spring("????.######..#####. 1,6,5").CalculatePermutations() == 4);
         Debug.Assert(new Spring("?###???????? 3,2,1").CalculatePermutations() == 10);
        
         var result = springs.Sum(s => s.CalculatePermutations());

        return result;
    }
    
    private static readonly ConcurrentDictionary<string, long> Cache = new();

    private record Spring
    {
        public string Springs { get; init; }
        public List<int> DamagedSprings { get; init; } = [];
        
        public Spring(string input, bool blowUp = false)
        {
            var split = input.Split();

            Springs = split[0];
            if (blowUp)
            {
                for (int i = 0; i < 4; i++)
                {
                    Springs += $"?{split[0]}";
                }
            }
            
            var damagedSprings = split[1].Split(',').Select(int.Parse).ToList();
            if (blowUp)
            {
                for (int i = 0; i < 5; i++)
                {
                    DamagedSprings.AddRange(damagedSprings);                    
                }
            }
            else
            {
                DamagedSprings = damagedSprings;
            }
        }

        public long CalculatePermutations()
        {
            var result = CountPermutations([..Springs, '.'], new Queue<int>(DamagedSprings), 0);
            
            return result;
        }

        private static long CountPermutations(List<char> springs, Queue<int> damagedSprings, int currentStreak)
        {
            var hash = $"{string.Join("", springs)}|{string.Join(",", damagedSprings)}|{currentStreak}";
            
            if (Cache.TryGetValue(hash, out long permutations))
            {
                return permutations;
            }
            
            var result = CountPermutationsCore(springs, damagedSprings, currentStreak);
            Cache[hash] = result;
            return result;
        }
        
        private static long CountPermutationsCore(List<char> springs, Queue<int> damagedSprings, int currentStreak)
        {
            for (var index = 0; index < springs.Count; index++)
            {
                char c = springs[index];
                if (c == '#')
                {
                    currentStreak++;
                }
                else if (c == '.')
                {   
                    if (currentStreak > 0)
                    {
                        if (damagedSprings.Count == 0)
                            return 0;
                        
                        if (currentStreak == damagedSprings.Dequeue())
                        {
                            currentStreak = 0;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else if (c == '?')
                {
                    var remainingSprings = springs[(index + 1)..];

                    var pathOne = CountPermutations(['.', ..remainingSprings], new Queue<int>(damagedSprings), currentStreak);
                    var pathTwo = CountPermutations(['#', ..remainingSprings], new Queue<int>(damagedSprings), currentStreak);
                    
                    return pathOne + pathTwo;
                }
            }
            
            return damagedSprings.Count == 0 ? 1 : 0;
        }
    }
    
    public object SolveSecondPart(Input input)
    {
        var springs = input
            .GetLines()
            .Select(l => new Spring(l, true))
            .ToList();
        
        // Debug.Assert(new Spring("???.### 1,1,3", true).CalculatePermutations() == 1);
        // Debug.Assert(new Spring(".??..??...?##. 1,1,3", true).CalculatePermutations() == 16384);
        // Debug.Assert(new Spring("?#?#?#?#?#?#?#? 1,3,1,6", true).CalculatePermutations() == 1);
        // Debug.Assert(new Spring("????.#...#... 4,1,1", true).CalculatePermutations() == 16);
        // Debug.Assert(new Spring("????.######..#####. 1,6,5", true).CalculatePermutations() == 2500);
        // Debug.Assert(new Spring("?###???????? 3,2,1", true).CalculatePermutations() == 506250);
        
        var result = new ConcurrentBag<long>();

        Parallel.ForEach(springs, s =>
        {
            result.Add(s.CalculatePermutations());
            Console.WriteLine($"{result.Count} of {springs.Count} solved.");
        });

        return result.Sum();
    }
}