using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions2019.Shared.Computer
{
    public enum Instructions
    {
        ADD = 1,
        MULTIPLY = 2,
        SAVE = 3,
        OUTPUT = 4,
        JIT = 5,
        JIF = 6,
        LT = 7,
        EQ = 8,
        HCF = 99
    }
    
    public enum Mode
    {
        POSITION = 0,
        IMMEDIATE = 1
    }

    public class Computer
    {
        private readonly int[] _program;
        private readonly List<int> _output;

        public Computer()
        {
            
        }
        
        public Computer(int[] instructionsParameter)
        {
            _program = instructionsParameter;
            _output = new List<int>();
        }

        public List<int> Run(int input)
        {
            var pointer = 0;

            while (_program[pointer] != (int)Instructions.HCF)
            {
                var instruction = new Instruction(_program, pointer);
                
                switch (instruction.Type)
                {
                    case Instructions.ADD:
                    {
                        _program[instruction.Inputs[2]] = GetValue(instruction.Inputs[0], instruction.Modes[0]) + GetValue(instruction.Inputs[1], instruction.Modes[1]);
                        break;
                    }
                    case Instructions.MULTIPLY:
                    {
                        _program[instruction.Inputs[2]] = GetValue(instruction.Inputs[0], instruction.Modes[0]) * GetValue(instruction.Inputs[1], instruction.Modes[1]);
                        break;
                    }
                    case Instructions.SAVE:
                    {
                        _program[instruction.Inputs[0]] = input;
                        break;
                    }
                    case Instructions.OUTPUT:
                    {
                        _output.Add(GetValue(instruction.Inputs[0], instruction.Modes[0]));
                        break;
                    }
                    case Instructions.JIT:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) != 0)
                        {
                            pointer = GetValue(instruction.Inputs[1], instruction.Modes[1]);
                            continue;
                        }
                        break;
                    }
                    case Instructions.JIF:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) == 0)
                        {
                            pointer = GetValue(instruction.Inputs[1], instruction.Modes[1]);
                            continue;
                        }
                        break;
                    }
                    case Instructions.LT:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) < GetValue(instruction.Inputs[1], instruction.Modes[1]))
                            _program[instruction.Inputs[2]] = 1;
                        else _program[instruction.Inputs[2]] = 0;
                        break;
                    }
                    case Instructions.EQ:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) == GetValue(instruction.Inputs[1], instruction.Modes[1]))
                            _program[instruction.Inputs[2]] = 1;
                        else _program[instruction.Inputs[2]] = 0;
                        break;
                    }
                    default:
                    {
                        throw new Exception($"Unexpected opcode encountered: {_program[pointer]}");
                    }
                }

                pointer += instruction.Length;
            }

            return _output;
        }
        
        public int GetValue(int pointer, Mode opType)
        {
            return opType switch
            {
                Mode.POSITION => _program[pointer],
                Mode.IMMEDIATE => pointer,
                _ => throw new Exception("invalid optype"),
            };
        }
        
        public int ComputeSimple(int[] instructionsParameter, int noun, int verb)
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