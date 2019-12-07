using System;
using System.Collections.Generic;
using System.Linq;

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
    
    public enum State
    {
        NOTSTARTED = 0,
        WAITING = 1,
        FINISHED = 99
    }

    public class Computer
    {
        private int[] _program;
        private readonly int[] _original;
        private List<int> _output;
        private List<int> _input = new List<int>();
        public State State = State.NOTSTARTED;
        private int _pointer = 0;
        private int _inputPointer = 0;

        public Computer()
        {
            
        }
        
        public Computer(int[] instructionsParameter)
        {
            _program = instructionsParameter;
            
            _original = new int[instructionsParameter.Length];
            instructionsParameter.CopyTo(_original, 0);

            _output = new List<int>();
        }

        public List<int> Run(int input)
        {
            _input.Add(input);
            return Run();
        }

        public List<int> Run(int[] input)
        {
            _input.AddRange(input);
            return Run();
        }
        
        public List<int> Run()
        {
            while (_program[_pointer] != (int)Instructions.HCF)
            {
                var instruction = new Instruction(_program, _pointer);
                
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
                        if (_input.Count > _inputPointer)
                        {
                            _program[instruction.Inputs[0]] = _input[_inputPointer];
                            _inputPointer++;
                        }
                        else
                        {
                            State = State.WAITING;
                            return _output;
                        }

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
                            _pointer = GetValue(instruction.Inputs[1], instruction.Modes[1]);
                            continue;
                        }
                        break;
                    }
                    case Instructions.JIF:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) == 0)
                        {
                            _pointer = GetValue(instruction.Inputs[1], instruction.Modes[1]);
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
                        throw new Exception($"Unexpected opcode encountered: {_program[_pointer]}");
                    }
                }

                _pointer += instruction.Length;
            }

            State = State.FINISHED;
            return _output;
        }

        public int GetOutput()
        {
            var result = _output.First();
            _output.RemoveAt(0);
            return result;
        }

        private int GetValue(int pointer, Mode opType)
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