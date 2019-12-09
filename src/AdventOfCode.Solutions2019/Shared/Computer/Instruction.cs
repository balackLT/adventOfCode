using System.Collections.Generic;

namespace AdventOfCode.Solutions2019.Shared.Computer
{
    public class Instruction
    {
        public readonly Instructions Type;
        public readonly int Length;
        public readonly long[] Inputs = new long[4];
        public readonly Mode[] Modes = new Mode[4];
        
        public Instruction(List<long> instructions, int pointer)
        {
            var current = instructions[pointer];
            var instruction = current % 100;

            switch (instruction)
            {
                case (int) Instructions.ADD:
                    Type = Instructions.ADD;
                    Length = 4;
                    break;
                case (int) Instructions.MULTIPLY:
                    Type = Instructions.MULTIPLY;
                    Length = 4;
                    break;
                case (int) Instructions.HCF:
                    Type = Instructions.HCF;
                    Length = 0;
                    return;
                case (int) Instructions.SAVE:
                    Type = Instructions.SAVE;
                    Length = 2;
                    break;
                case (int) Instructions.OUTPUT:
                    Type = Instructions.OUTPUT;
                    Length = 2;
                    break;
                case (int) Instructions.JIF:
                    Type = Instructions.JIF;
                    Length = 3;
                    break;
                case (int) Instructions.JIT:
                    Type = Instructions.JIT;
                    Length = 3;
                    break;
                case (int) Instructions.LT:
                    Type = Instructions.LT;
                    Length = 4;
                    break;
                case (int) Instructions.EQ:
                    Type = Instructions.EQ;
                    Length = 4;
                    break;
                case (int) Instructions.RELB:
                    Type = Instructions.RELB;
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
}