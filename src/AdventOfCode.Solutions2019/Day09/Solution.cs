using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;

namespace AdventOfCode.Solutions2019.Day09;

public class Solution : ISolution
{
    public int Day { get; } = 9;
        
    public string SolveFirstPart(Input input)
    {
        Test(new long[] {1102,34915192,34915192,7,4,7,99,0}, 1219070632396864);
        Test(new long[] {104,1125899906842624,99}, 1125899906842624);
        Test(new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99}, new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99});
            
        var program = input.GetLineAsLongArray();
            
        var computer = new Computer(program);

        computer.Run(1);
            
        Debug.Assert(computer.State == State.FINISHED);
        Debug.Assert(computer.Output.Count == 1);
            
        return computer.GetOutput().ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var program = input.GetLineAsLongArray();
            
        var computer = new Computer(program);

        computer.Run(2);
            
        Debug.Assert(computer.State == State.FINISHED);
        Debug.Assert(computer.Output.Count == 1);
            
        return computer.GetOutput().ToString();
    }

    private void Test(long[] program, long expected)
    {
        var computer = new Computer(program);
        computer.Run();
        var output = computer.GetOutput();
            
        Debug.Assert(computer.State == State.FINISHED);
        Debug.Assert(output == expected);
    }
        
    private void Test(long[] program, long[] expected)
    {
        var computer = new Computer(program);
        computer.Run();
            
        Debug.Assert(computer.State == State.FINISHED);
        Debug.Assert(computer.Output.SequenceEqual(expected));
    }
}