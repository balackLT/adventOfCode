using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions2020.Day08
{
    public class Computer
    {
        public List<Instruction> Instructions { get; init; }

        public int Accumulator { get; set;  }
        private int _pointer;

        public Computer(List<Instruction> instructions)
        {
            Instructions = instructions;
        }

        public int Execute()
        {
            var executedInstructions = new HashSet<int>();

            while (true)
            {
                if (_pointer >= Instructions.Count)
                    return 0; // normal exit
                
                var instruction = Instructions[_pointer];
                
                if (!executedInstructions.Add(_pointer))
                    return -1; // "infinite loop"

                switch (instruction.Code)
                {
                    case "acc":
                        Accumulator += instruction.Number;
                        _pointer++;
                        break;
                    case "jmp":
                        _pointer += instruction.Number;
                        break;
                    case "nop":
                        _pointer++;
                        break;
                }
            }
        }
    }
}