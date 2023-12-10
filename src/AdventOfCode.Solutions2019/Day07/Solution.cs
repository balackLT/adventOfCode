using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;

namespace AdventOfCode.Solutions2019.Day07;

public class Solution : ISolution
{
    public int Day { get; } = 7;
        
    public object SolveFirstPart(Input input)
    {
        var program = input.GetLineAsLongArray();

        Test(new long[]{3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0}, 4, 3, 2, 1, 0, 43210);
        Test(new long[]{3,23,3,24,1002,24,10,24,1002,23,-1,23, 101,5,23,23,1,24,23,23,4,23,99,0,0}, 0, 1, 2,3, 4, 54321);
        Test(new long[]{3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0},1,0,4,3,2,65210);
            
        long maxOutput = 0;

        for (int A = 0; A < 5; A++)
        {
            for (int B = 0; B < 5; B++)
            {
                for (int C = 0; C < 5; C++)
                {
                    for (int D = 0; D < 5; D++)
                    {
                        for (int E = 0; E < 5; E++)
                        {
                            var ampPhases = new int[] {A, B, C, D, E };
                            if (ampPhases.Distinct().Count() < 5)
                                continue;
                                
                            var result = AmplifyFeedback(program, A, B, C, D, E);
                                
                            if (result > maxOutput)
                                maxOutput = result;
                        }
                    }
                }
            }
        }
            
        return maxOutput.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var program = input.GetLineAsLongArray();
            
        long maxOutput = 0;

        for (int A = 5; A < 10; A++)
        {
            for (int B = 5; B < 10; B++)
            {
                for (int C = 5; C < 10; C++)
                {
                    for (int D = 5; D < 10; D++)
                    {
                        for (int E = 5; E < 10; E++)
                        {
                            var ampPhases = new int[] {A, B, C, D, E };
                            if (ampPhases.Distinct().Count() < 5)
                                continue;
                                
                            var result = AmplifyFeedback(program, A, B, C, D, E);
                                
                            if (result > maxOutput)
                                maxOutput = result;
                        }
                    }
                }
            }
        }
            
        return maxOutput.ToString();
    }

    private long AmplifyFeedback(long[] program, int A, int B, int C, int D, int E)
    {
        var computers = new Computer[5];
        for (var i = 0; i < 5; i++)
        {
            computers[i] = new Computer(program);
        }

        computers[0].Run(new long[] {A, 0});
        var outputA = computers[0].GetOutput();
        computers[1].Run(new long[] {B, outputA});
        var outputB = computers[1].GetOutput();
        computers[2].Run(new long[] {C, outputB});
        var outputC = computers[2].GetOutput();
        computers[3].Run(new long[] {D, outputC});
        var outputD = computers[3].GetOutput();
        computers[4].Run(new long[] {E, outputD});
        var outputE = computers[4].GetOutput();

        if (computers[4].State == State.FINISHED)
            return outputE;
            
            
        while (true)
        {
            computers[0].Run(outputE);
            outputA = computers[0].GetOutput();
            computers[1].Run(outputA);
            outputB = computers[1].GetOutput();
            computers[2].Run(outputB);
            outputC = computers[2].GetOutput();
            computers[3].Run(outputC);
            outputD = computers[3].GetOutput();
            computers[4].Run(outputD);
            outputE = computers[4].GetOutput();
                
            if (computers[4].State == State.FINISHED)
                return outputE;
        }
    }
        
    private void Test(long[] program, int A, int B, int C, int D, int E, int result)
    {
        var test = AmplifyFeedback(program, A, B, C, D, E);
            
        Debug.Assert(result == test);
    }
}