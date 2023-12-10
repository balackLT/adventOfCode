using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;

namespace AdventOfCode.Solutions2019.Day17;

public class Solution : ISolution
{
    public int Day { get; } = 17;
        
    public object SolveFirstPart(Input input)
    {
        var program = input.GetLineAsLongArray();
            
        var walkways = new WalkWayMap(program);
        walkways.ConstructMap();
        // walkways.Draw();

        return walkways.CalculateAlignmentParameter().ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var program = input.GetLineAsLongArray();
            
        var walkways = new WalkWayMap(program);

        walkways.ConstructMap();
            
        var traversal = walkways.ConstructMapTraversalString();
            
        // TODO: it is MUCH easier to split the string by hand. One day TODO write a splitting algorithm
        // My traversal path: L12,L8,L8,L12,R4,L12,R6,L12,L8,L8,R4,L12,L12,R6,L12,R4,L12,R6,L12,L8,L8,R4,L12,L12,R6,L12,L8,L8,R4,L12,L12,R6,L12,R4,L12,R6
        var A = "L,12,L,8,L,8".Select(c => (int)c);
        var B = "L,12,R,4,L,12,R,6".Select(c => (int)c);
        var C = "R,4,L,12,L,12,R,6".Select(c => (int)c);
        var routine = "A,B,A,C,B,A,C,A,C,B".Select(c => (int)c);
            
        Debug.Assert(A.Count() <= 20);
        Debug.Assert(B.Count() <= 20);
        Debug.Assert(C.Count() <= 20);
        Debug.Assert(routine.Count() <= 20);

        var robot = new Computer(program);
        robot.SetMemoryAddress(0, 2);

        var robotInput = new List<int>();
        robotInput.AddRange(routine.Select(c => (int)c));
        robotInput.Add(10);
            
        robotInput.AddRange(A);
        robotInput.Add(10);
            
        robotInput.AddRange(B.Select(c => (int)c));
        robotInput.Add(10);
            
        robotInput.AddRange(C.Select(c => (int)c));
        robotInput.Add(10);
            
        robotInput.Add((int)'n');
        robotInput.Add(10);

        robot.Run(robotInput.Select(i => (long)i));

        var result = robot.Output.Last();
        Debug.Assert(robot.State == State.FINISHED);
            
        return result.ToString();
    }
}