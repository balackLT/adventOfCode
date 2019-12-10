using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions2019.Shared.Computer
{
    public class Computer
    {
        public State State { get; private set; } = State.NOTSTARTED;
        public readonly List<long> Output = new List<long>();
        
        private readonly List<long> _program;
        private readonly Queue<long> _input = new Queue<long>();
        
        private int _pointer = 0;
        private int _relativeBase = 0;

        public Computer(IEnumerable<long> instructionsParameter)
        {
            _program = instructionsParameter.ToList();
        }

        public State Run(long input)
        {
            _input.Enqueue(input);
            return Run();
        }

        public State Run(IEnumerable<long> input)
        {
            foreach (var i in input)
            {
                _input.Enqueue(i);
            }
            
            return Run();
        }
        
        public State Run()
        {
            while (_program[_pointer] != (int)InstructionType.HCF)
            {
                var instruction = new Instruction(_program, _pointer);
                
                switch (instruction.Type)
                {
                    case InstructionType.ADD:
                    {
                        var value = GetValue(instruction, 0) + GetValue(instruction, 1);
                        SetValue(instruction.Inputs[2], value, instruction.Modes[2]);
                        break;
                    }
                    case InstructionType.MULTIPLY:
                    {
                        var value = GetValue(instruction, 0) * GetValue(instruction, 1);
                        SetValue(instruction.Inputs[2], value, instruction.Modes[2]);
                        break;
                    }
                    case InstructionType.SAVE:
                    {
                        if (_input.Count > 0)
                        {
                            SetValue(instruction.Inputs[0], _input.Dequeue(), instruction.Modes[0]);
                        }
                        else
                        {
                            State = State.WAITING;
                            return State;
                        }

                        break;
                    }
                    case InstructionType.OUTPUT:
                    {
                        var value = GetValue(instruction, 0);
                        Output.Add(value);
                        break;
                    }
                    case InstructionType.JIT:
                    {
                        if (GetValue(instruction, 0) != 0)
                        {
                            _pointer = (int) GetValue(instruction, 1);
                            continue;
                        }
                        break;
                    }
                    case InstructionType.JIF:
                    {
                        if (GetValue(instruction, 0) == 0)
                        {
                            _pointer = (int) GetValue(instruction, 1);
                            continue;
                        }
                        break;
                    }
                    case InstructionType.LT:
                    {
                        if (GetValue(instruction, 0) < GetValue(instruction, 1))
                            SetValue(instruction.Inputs[2], 1, instruction.Modes[2]);
                        else SetValue(instruction.Inputs[2], 0, instruction.Modes[2]);
                        break;
                    }
                    case InstructionType.EQ:
                    {
                        if (GetValue(instruction, 0) == GetValue(instruction, 1))
                            SetValue(instruction.Inputs[2], 1, instruction.Modes[2]);
                        else SetValue(instruction.Inputs[2], 0, instruction.Modes[2]);
                        break;
                    }
                    case InstructionType.RELB:
                    {
                        _relativeBase += (int) GetValue(instruction, 0);
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
            return State;
        }

        public long GetOutput()
        {
            var result = Output.First();
            Output.RemoveAt(0);
            return result;
        }

        private long GetValue(Instruction instruction, int position)
        {
            return instruction.Modes[position] switch
            {
                Mode.POSITION => GetValueAtPosition((int) instruction.Inputs[position]),
                Mode.IMMEDIATE => instruction.Inputs[position],
                Mode.RELATIVE => GetValueAtPosition((int) instruction.Inputs[position] + _relativeBase),
                _ => throw new Exception("invalid optype provided to get value")
            };
        }
        
        private void SetValue(long pointer, long value, Mode opType)
        {
            if (_program.Count < pointer + _relativeBase)
                _program.AddRange(Enumerable.Repeat((long)0, (int) pointer + _relativeBase));
            
            _ = opType switch
            {
                Mode.POSITION => _program[(int) pointer] = value,
                Mode.RELATIVE => _program[(int) pointer + _relativeBase] = value,
                _ => throw new Exception("Invalid mode provided to set value")
            };
        }

        private long GetValueAtPosition(int position)
        {
            if (_program.Count < position + _relativeBase)
                _program.AddRange(Enumerable.Repeat((long)0, position + _relativeBase));

            return _program[position];
        }
    }
}