using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day05
{
    public class Solution : ISolution
    {
        public int Day { get; } = 5;

        private readonly List<char> _vowels = "aeiou".ToList(); 
        private readonly List<string> _badStrings = new List<string>{"ab", "cd", "pq", "xy"};
        
        public string SolveFirstPart(Input input)
        {
            var strings = input.GetLines();

            var result = strings.Count(IsNice);
            
            return result.ToString();
        }

        private bool IsNice(string input)
        {
            if (_vowels.Sum(v => input.Count(c => c == v)) < 3)
                return false;
            
            if (!input.Any(c => input.Contains($"{c}{c}")))
                return false;

            if (_badStrings.Any(input.Contains))
                return false;
            
            return true;
        }

        public string SolveSecondPart(Input input)
        {
            var strings = input.GetLines();
            
            Debug.Assert(IsNicer("qjhvhtzxzqqjkmpb"));
            Debug.Assert(IsNicer("xxyxx"));
            Debug.Assert(IsNicer("uurcxstgmygtbstg") == false);
            Debug.Assert(IsNicer("ieodomkazucvgmuy") == false);
            Debug.Assert(IsNicer("aaa") == false);
            Debug.Assert(IsNicer("xyxyx"));

            var result = strings.Count(IsNicer);
            
            return result.ToString();
        }

        private bool IsNicer(string input)
        {
            var between = false;
            var repeats = false;

            var iterator = 0;
            
            foreach (var letter in input)
            {
                if (input.Length <= iterator + 2)
                    break;

                if (repeats == false)
                {
                    var pair = letter.ToString() + input[iterator + 1].ToString();
                    var tempString = input[(iterator + 2)..];
                    if (tempString.Contains(pair))
                        repeats = true;
                }

                if (between == false)
                {
                    if (input[iterator + 2] == letter)
                        between = true;
                }

                iterator++;
            }
            
            return between && repeats;
        }
    }
}