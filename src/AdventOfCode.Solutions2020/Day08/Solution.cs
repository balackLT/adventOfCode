using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day08;

public class Solution : ISolution
{
    public int Day { get; } = 8;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLinesByRegex(@"(\w{3})\s([-+]\d+)");

        var instructions = lines.Select(l => new Instruction
        {
            Code = l[1],
            Number = int.Parse(l[2])
        }).ToList();

        var computer = new Computer(instructions);
            
        computer.Execute();
            
        return computer.Accumulator.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLinesByRegex(@"(\w{3})\s([-+]\d+)");

        var instructions = lines.Select(l => new Instruction
        {
            Code = l[1],
            Number = int.Parse(l[2])
        }).ToList();

        var computers = new List<Computer>();
            
        for (var i = 0; i < instructions.Count; i++)
        {
            if (instructions[i].Code == "acc")
                continue;

            var instructionCopy = instructions
                .Select(j => new Instruction
                {
                    Code = j.Code,
                    Number = j.Number
                }).ToList();

            instructionCopy[i].Code = instructionCopy[i].Code switch
            {
                "nop" => "jmp",
                "jmp" => "nop",
                _ => instructionCopy[i].Code
            };

            computers.Add(new Computer(instructionCopy));
        }

        var result = 0;
            
        foreach (var modifiedComputer in computers)
        {
            if (modifiedComputer.Execute() == 0)
            {
                result = modifiedComputer.Accumulator;
                break;
            }
        }
            
        return result.ToString();
    }
        
}