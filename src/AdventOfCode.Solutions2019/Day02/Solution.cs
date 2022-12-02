using System;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;

namespace AdventOfCode.Solutions2019.Day02;

public class Solution : ISolution
{
    public int Day { get; } = 2;
        
    public string SolveFirstPart(Input input)
    {
        var instructions = input.GetLineAsIntArray();
            
        var result = ComputeSimple(instructions, 12, 2);
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var instructions = input.GetLineAsIntArray();
        var expectedResult = 19690720;

        for (int noun = 0; noun < 100; noun++)
        {
            for (int verb = 0; verb < 100; verb++)
            {
                if (ComputeSimple(instructions, noun, verb) == expectedResult)
                    return (100 * noun + verb).ToString();
            }
        }

        throw new Exception("Solution not found :(");
    }

    private int ComputeSimple(int[] instructionsParameter, int noun, int verb)
    {
        var pointer = 0;
        int[] instructions = new int[instructionsParameter.Length];
        instructionsParameter.CopyTo(instructions, 0);
            
        instructions[1] = noun;
        instructions[2] = verb;
            
        while (instructions[pointer] != (int)InstructionType.HCF)
        {
            instructions[instructions[pointer + 3]] = instructions[pointer] switch
            {
                (int) InstructionType.ADD => (instructions[instructions[pointer + 1]] + instructions[instructions[pointer + 2]]),
                (int) InstructionType.MULTIPLY => (instructions[instructions[pointer + 1]] * instructions[instructions[pointer + 2]]),
                _ => throw new Exception($"Unexpected opcode encountered: {instructions[pointer]}")
            };

            pointer += 4;
        }

        return instructions[0];
    }
}