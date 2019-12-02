using System;

namespace AdventOfCode.Solutions2019.Shared.Computer
{
    public enum Instructions
    {
        ADD = 1,
        MULTIPLY = 2,
        HCF = 99
    }
    
    public class Computer
    {
        public int Compute(int[] instructionsParameter, int noun, int verb)
        {
            var pointer = 0;
            int[] instructions = new int[instructionsParameter.Length];
            instructionsParameter.CopyTo(instructions, 0);
            
            instructions[1] = noun;
            instructions[2] = verb;
            
            while (instructions[pointer] != (int)Instructions.HCF)
            {
                instructions[instructions[pointer + 3]] = instructions[pointer] switch
                {
                    (int) Instructions.ADD => (instructions[instructions[pointer + 1]] + instructions[instructions[pointer + 2]]),
                    (int) Instructions.MULTIPLY => (instructions[instructions[pointer + 1]] * instructions[instructions[pointer + 2]]),
                    _ => throw new Exception($"Unexpected opcode encountered: {instructions[pointer]}")
                };

                pointer += 4;
            }

            return instructions[0];
        }
    }
}