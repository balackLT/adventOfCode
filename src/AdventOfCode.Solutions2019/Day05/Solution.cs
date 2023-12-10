using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;

namespace AdventOfCode.Solutions2019.Day05;

public class Solution : ISolution
{
    public int Day { get; } = 5;
        
    public object SolveFirstPart(Input input)
    {
        var instructions = input.GetLineAsLongArray();
        //instructions = new int[] {3,0,4,0,99};
        //instructions = new int[] {1002,4,3,4,33};
            
        var computer = new Computer(instructions);

        var output = computer.Run(1);

        return string.Join(',', output);
    }

    public object SolveSecondPart(Input input)
    {
        var instructions = input.GetLineAsLongArray();
        // instructions = new int[] {3,0,4,0,99};
        // instructions = new int[] {3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99};
            
        var computer = new Computer(instructions);

        var output = computer.Run(5);

        return string.Join(',', output);
    }
}