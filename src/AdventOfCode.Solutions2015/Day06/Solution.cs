using System;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2015.Day06;

public struct Instruction
{
    public string Type;
    public string Direction;
    public Coordinate Left;
    public Coordinate Right;
}
    
public class Solution : ISolution
{
    public int Day { get; } = 6;

    public string SolveFirstPart(Input input)
    {
        var instructions = input
            .GetLines()
            .Select(l => l.Split(' '))
            .Select(ParseInput);
            
        var map = new Map<bool>(false);

        foreach (var instruction in instructions)
        {
            for (var y = instruction.Left.Y; y <= instruction.Right.Y; y++)
            {
                for (var x = instruction.Left.X; x <= instruction.Right.X; x++)
                {
                    var coordinate = new Coordinate(x, y);

                    map[coordinate] = instruction.Type switch
                    {
                        "toggle" => !map[coordinate],
                        "turn" when instruction.Direction == "off" => false,
                        "turn" when instruction.Direction == "on" => true,
                        _ => throw new Exception($"Unknown instruction")
                    };
                }
            }
        }
            
        var result = map.InternalMap.Count(h => h.Value);

        return result.ToString();
    }

    private Instruction ParseInput(string[] input)
    {

        var instr = new Instruction {Type = input[0]};

        if (instr.Type == "turn")
        {
            instr.Direction = input[1];
            instr.Left = new Coordinate(input[2]);
            instr.Right = new Coordinate(input[4]);
        }
        else
        {
            instr.Left = new Coordinate(input[1]);
            instr.Right = new Coordinate(input[3]);
        }

        return instr;
    }

    public string SolveSecondPart(Input input)
    {
        var instructions = input
            .GetLines()
            .Select(l => l.Split(' '))
            .Select(ParseInput);
            
        var map = new Map<int>(0);

        foreach (var instruction in instructions)
        {
            for (var y = instruction.Left.Y; y <= instruction.Right.Y; y++)
            {
                for (var x = instruction.Left.X; x <= instruction.Right.X; x++)
                {
                    var coordinate = new Coordinate(x, y);

                    map[coordinate] = instruction.Type switch
                    {
                        "toggle" => map[coordinate] + 2,
                        "turn" when instruction.Direction == "off" => map[coordinate] > 0 ? map[coordinate] - 1 : 0,
                        "turn" when instruction.Direction == "on" => map[coordinate] + 1,
                        _ => throw new Exception($"Unknown instruction")
                    };
                }
            }
        }
            
        var result = map.InternalMap.Sum(h => h.Value);

        return result.ToString();
    }
}