using System;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;

namespace AdventOfCode.Solutions2019.Day02
{
    public class Solution : ISolution
    {
        public int Day { get; } = 2;
        
        public string SolveFirstPart(Input input)
        {
            var instructions = input.GetLineAsIntArray();
            var computer = new Computer();
            
            var result = computer.Compute(instructions, 12, 2);
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var instructions = input.GetLineAsIntArray();
            var computer = new Computer();
            var expectedResult = 19690720;

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    if (computer.Compute(instructions, noun, verb) == expectedResult)
                        return (100 * noun + verb).ToString();
                }
            }

            throw new Exception("Solution not found :(");
        }
    }
}