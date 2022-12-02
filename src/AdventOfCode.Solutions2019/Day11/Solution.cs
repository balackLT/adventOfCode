using System;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2019.Day11;

public class Solution : ISolution
{
    public int Day { get; } = 11;

    private const char WHITE = '█';
    private const char BLACK = '.';
    private const char EMPTY = ' ';
        
    public string SolveFirstPart(Input input)
    {
        var program = input.GetLineAsLongArray();

        var map = new Map<char>(EMPTY);
        var brain = new Computer(program);
        var robot = new Robot(brain, new Coordinate(0, 0));
            
        while (true)
        {
            var localSpace = map[robot.Location];
            var robotInput = localSpace == WHITE ? 1 : 0;

            var previousLocation = robot.Location;
            var robotOutput = robot.DoStuff(robotInput);
                
            var color = robotOutput switch
            {
                0 => BLACK,
                1 => WHITE,
                _ => throw new Exception("Invalid color computed")
            };

            map[previousLocation] = color;

            if (robot.Finished())
                break;
        }

        var result = map.InternalMap.Count(m => m.Value != EMPTY);
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var program = input.GetLineAsLongArray();

        var map = new Map<char>(EMPTY);
        var brain = new Computer(program);
        var robot = new Robot(brain, new Coordinate(0, 0));

        map[new Coordinate(0, 0)] = WHITE;
            
        while (true)
        {
            var localSpace = map[robot.Location];
            var robotInput = localSpace == WHITE ? 1 : 0;

            var previousLocation = robot.Location;
            var robotOutput = robot.DoStuff(robotInput);
                
            var color = robotOutput switch
            {
                0 => BLACK,
                1 => WHITE,
                _ => throw new Exception("Invalid color computed")
            };

            map[previousLocation] = color;

            if (robot.Finished())
                break;
        }

        map.PrintMapFlipY();
            
        return 0.ToString();
    }
}