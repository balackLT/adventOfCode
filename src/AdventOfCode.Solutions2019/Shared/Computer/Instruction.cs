using System.Collections.Generic;

namespace AdventOfCode.Solutions2019.Shared.Computer;

public class Instruction
{
    public readonly InstructionType Type;
    public readonly int Length;
    public readonly long[] Inputs = new long[4];
    public readonly Mode[] Modes = new Mode[4];
        
    public Instruction(List<long> instructions, int pointer)
    {
        var current = instructions[pointer];
        var instruction = current % 100;

        switch (instruction)
        {
            case (int) InstructionType.ADD:
                Type = InstructionType.ADD;
                Length = 4;
                break;
            case (int) InstructionType.MULTIPLY:
                Type = InstructionType.MULTIPLY;
                Length = 4;
                break;
            case (int) InstructionType.HCF:
                Type = InstructionType.HCF;
                Length = 0;
                return;
            case (int) InstructionType.SAVE:
                Type = InstructionType.SAVE;
                Length = 2;
                break;
            case (int) InstructionType.OUTPUT:
                Type = InstructionType.OUTPUT;
                Length = 2;
                break;
            case (int) InstructionType.JIF:
                Type = InstructionType.JIF;
                Length = 3;
                break;
            case (int) InstructionType.JIT:
                Type = InstructionType.JIT;
                Length = 3;
                break;
            case (int) InstructionType.LT:
                Type = InstructionType.LT;
                Length = 4;
                break;
            case (int) InstructionType.EQ:
                Type = InstructionType.EQ;
                Length = 4;
                break;
            case (int) InstructionType.RELB:
                Type = InstructionType.RELB;
                Length = 2;
                break;
        }

        var input = 0;
        var modeList = current / 100;
        for (var i = pointer + 1; i < pointer + Length; i++)
        {
            Inputs[input] = instructions[i];
            Modes[input] = (Mode)(modeList % 10);
            modeList /= 10;
            input++;
        }
    }
}