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
        RELB = 9,
        HCF = 99
    }
    
    public enum Mode
    {
        POSITION = 0,
        IMMEDIATE = 1,
        RELATIVE = 2
    }
    
    public enum State
    {
        NOTSTARTED = 0,
        WAITING = 1,
        FINISHED = 99
    }

    public class Computer
    {
        private List<long> _program;
        private readonly long[] _original;
        public List<long> _output;
        private List<long> _input = new List<long>();
        public State State = State.NOTSTARTED;
        private int _pointer = 0;
        private int _inputPointer = 0;
        private int _relativeBase = 0;

        public Computer()
        {
            
        }
        
        public Computer(long[] instructionsParameter)
        {
            _program = instructionsParameter.ToList();
            
            _original = new long[instructionsParameter.Length];
            instructionsParameter.CopyTo(_original, 0);

            _output = new List<long>();
        }

        public List<long> Run(long input)
        {
            _input.Add(input);
            return Run();
        }

        public List<long> Run(long[] input)
        {
            _input.AddRange(input);
            return Run();
        }
        
        public List<long> Run()
        {
            while (_program[_pointer] != (int)Instructions.HCF)
            {
                var instruction = new Instruction(_program, _pointer);
                
                switch (instruction.Type)
                {
                    case Instructions.ADD:
                    {
                        var value = GetValue(instruction.Inputs[0], instruction.Modes[0]) + GetValue(instruction.Inputs[1], instruction.Modes[1]);
                        SetValue(instruction.Inputs[2], value, instruction.Modes[2]);
                        break;
                    }
                    case Instructions.MULTIPLY:
                    {
                        var value = GetValue(instruction.Inputs[0], instruction.Modes[0]) * GetValue(instruction.Inputs[1], instruction.Modes[1]);
                        SetValue(instruction.Inputs[2], value, instruction.Modes[2]);
                        break;
                    }
                    case Instructions.SAVE:
                    {
                        if (_input.Count > _inputPointer)
                        {
                            SetValue(instruction.Inputs[0], _input[_inputPointer], instruction.Modes[0]);
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
                        var value = GetValue(instruction.Inputs[0], instruction.Modes[0]);
                        _output.Add(value);
                        break;
                    }
                    case Instructions.JIT:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) != 0)
                        {
                            _pointer = (int) GetValue(instruction.Inputs[1], instruction.Modes[1]);
                            continue;
                        }
                        break;
                    }
                    case Instructions.JIF:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) == 0)
                        {
                            _pointer = (int) GetValue(instruction.Inputs[1], instruction.Modes[1]);
                            continue;
                        }
                        break;
                    }
                    case Instructions.LT:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) < GetValue(instruction.Inputs[1], instruction.Modes[1]))
                            SetValue(instruction.Inputs[2], 1, instruction.Modes[2]);
                        else SetValue(instruction.Inputs[2], 0, instruction.Modes[2]);
                        break;
                    }
                    case Instructions.EQ:
                    {
                        if (GetValue(instruction.Inputs[0], instruction.Modes[0]) == GetValue(instruction.Inputs[1], instruction.Modes[1]))
                            SetValue(instruction.Inputs[2], 1, instruction.Modes[2]);
                        else SetValue(instruction.Inputs[2], 0, instruction.Modes[2]);
                        break;
                    }
                    case Instructions.RELB:
                    {
                        _relativeBase += (int) GetValue(instruction.Inputs[0], instruction.Modes[0]);
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

        public long GetOutput()
        {
            var result = _output.First();
            _output.RemoveAt(0);
            return result;
        }

        private long GetValue(long pointer, Mode opType)
        {
            switch (opType)
            {
                case Mode.POSITION:
                    return GetValueAtPosition((int) pointer);
                case Mode.IMMEDIATE:
                    return pointer;
                case Mode.RELATIVE:
                    return GetValueAtPosition((int) pointer + _relativeBase);
                default:
                    throw new Exception("invalid optype");
            }
        }

        private long GetValueAtPosition(int position)
        {
            if (_program.Count < position + _relativeBase)
                _program.AddRange(Enumerable.Repeat((long)0, position + _relativeBase));

            return _program[position];
        }

        private void SetValue(long pointer, long value, Mode opType)
        {
            if (_program.Count < pointer + _relativeBase)
                _program.AddRange(Enumerable.Repeat((long)0, (int) pointer + _relativeBase));
            
            switch (opType)
            {
                case Mode.POSITION:
                    _program[(int) pointer] = value;
                    break;
                case Mode.IMMEDIATE:
                    throw new Exception("THIS SHOULD NEVER HAPPEN TM");
                case Mode.RELATIVE:
                    _program[(int) pointer + _relativeBase] = value;
                    break;
                default:
                    throw new Exception("invalid optype");
            }
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