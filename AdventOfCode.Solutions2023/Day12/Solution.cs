using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day12;

public partial class Solution : ISolution
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

    private partial record Spring
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
            var onlyValidPermutations = YieldValidPermutations(Springs, "");

            var result =  onlyValidPermutations.Count();

            // dispose of cache
            _cache = new Dictionary<string, bool>();
            
            return result;
        }

        private bool IsValidButFaster(string permutation)
        {
            var damagedIndex = 0;
            var currentLength = 0;
            var expectedLength = DamagedSprings[damagedIndex];
            var finished = false;
            
            foreach (char c in permutation + '.')
            {
                if (c == '#')
                {
                    if (finished)
                        return false;
                    
                    currentLength++;
                }
                else if (currentLength > 0)
                {
                    if (currentLength == expectedLength)
                    {
                        damagedIndex++;
                        if (damagedIndex > DamagedSprings.Count - 1)
                        {
                            finished = true;
                        }
                        else
                        {
                            expectedLength = DamagedSprings[damagedIndex];
                        }
                    }
                    else
                    {
                        return false;
                    }
                    
                    currentLength = 0;
                }
            }

            return damagedIndex == DamagedSprings.Count;
        }
        
        private IEnumerable<string> YieldValidPermutations(string springs, string pathTaken)
        {
            for (var index = 0; index < springs.Length; index++)
            {
                char c = springs[index];
                if (c is '#' or '.')
                {
                    pathTaken += c;
                }
                else if (c == '?')
                {
                    if (IsValidPartial(pathTaken + '#'))
                    {
                        foreach (var permutation in YieldValidPermutations(springs[(index + 1)..], pathTaken + '#'))
                        {
                            yield return pathTaken + permutation;
                        }
                    }
                    
                    if (IsValidPartial(pathTaken + '.'))
                    {
                        foreach (var permutation in YieldValidPermutations(springs[(index + 1)..], pathTaken + '.'))
                        {
                            yield return pathTaken + permutation;
                        }
                    }
                    yield break;
                }
            }
            
            if (IsValidButFaster(pathTaken))
                yield return pathTaken;
        }

        private Dictionary<string, bool> _cache = new();

        private bool IsValidPartial(string pathTaken)
        {
            // if (_cache.TryGetValue(pathTaken, out bool result))
            // {
            //     return result;
            // }

            var result = IsValidPartialCore(pathTaken);
            //_cache[pathTaken] = result;
            return result;
        }

        private bool IsValidPartialCore(string pathTaken)
        {
            // count lengths of matches matched by SpringMatcher
            var springCounts = SpringMatcher()
                .Matches(pathTaken)
                .Select(m => m.Length)
                .ToList();

            if (springCounts.Count > DamagedSprings.Count)
            {
                return false;
            }
            
            for (var index = 0; index < springCounts.Count; index++)
            {
                int springCount = springCounts[index];
                
                if (springCount == DamagedSprings[index])
                {
                    continue;
                }

                if (index == springCounts.Count - 1)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        [GeneratedRegex(@"(\#+)")]
        private static partial Regex SpringMatcher();
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

        // long result = 0;
        // var i = 0;
        // var sw = Stopwatch.StartNew();
        // foreach (Spring s in springs)
        // {
        //     i++;
        //     Console.WriteLine($"{i} of {springs.Count} solved in {sw.Elapsed}.");
        //     result += s.CalculatePermutations();
        // }
        
        var result = new ConcurrentBag<long>();

        Parallel.ForEach(springs, s =>
        {
            result.Add(s.CalculatePermutations());
            Console.WriteLine($"{result.Count} of {springs.Count} solved.");
        });

        return result.Sum();
    }
}