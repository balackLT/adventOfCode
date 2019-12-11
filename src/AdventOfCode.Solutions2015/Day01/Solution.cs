using System;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day01
{
    public class Solution : ISolution
    {
        public int Day { get; } = 1;
        
        public string SolveFirstPart(Input input)
        {
            var instructions = input.GetAsString();

            var result = instructions.Count(i => i == '(') - instructions.Count(i => i == ')');

            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var instructions = input.GetAsString();

            var count = 0;
            var floor = 0;
            
            foreach (var instruction in instructions)
            {
                count++;

                floor += instruction switch
                {
                    '(' => 1,
                    ')' => -1,
                    _ => throw new Exception("")
                };

                if (floor == -1)
                    return count.ToString();
            }
            
            return 0.ToString();
        }
    }
}