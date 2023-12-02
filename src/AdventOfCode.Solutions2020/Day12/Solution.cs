using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day12;

public class Solution : ISolution
{
    public int Day { get; } = 12;

    public string SolveFirstPart(Input input)
    {
        var instructions = input
            .GetLinesByRegex(@"(\D)(\d+)")
            .Select(l => new {Instruction = l[1], Distance = int.Parse(l[2])});

        var heading = Coordinate.East;
        var location = Coordinate.Zero;

        foreach (var instruction in instructions)
        {
            switch (instruction.Instruction)
            {
                case "F":
                    location += heading * instruction.Distance;
                    break;
                case "R":
                {
                    for (var i = 0; i < instruction.Distance / 90; i++)
                    {
                        heading = heading.Turn(TurnDirection.Right);
                    }

                    break;
                }
                case "L":
                {
                    for (var i = 0; i < instruction.Distance / 90; i++)
                    {
                        heading = heading.Turn(TurnDirection.Left);
                    }

                    break;
                }
                case "N":
                    location += Coordinate.North * instruction.Distance;
                    break;
                case "S":
                    location += Coordinate.South * instruction.Distance;
                    break;
                case "E":
                    location += Coordinate.East * instruction.Distance;
                    break;
                case "W":
                    location += Coordinate.West * instruction.Distance;
                    break;
            }
        }

        var result = location.ManhattanDistance();
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var instructions = input
            .GetLinesByRegex(@"(\D)(\d+)")
            .Select(l => new {Instruction = l[1], Distance = int.Parse(l[2])});

        var location = Coordinate.Zero;
        var waypoint = new Coordinate(10, 1);

        foreach (var instruction in instructions)
        {
            switch (instruction.Instruction)
            {
                case "F":
                    location += waypoint * instruction.Distance;
                    break;
                case "R":
                {
                    for (var i = 0; i < instruction.Distance / 90; i++)
                    {
                        waypoint = waypoint.Turn(TurnDirection.Right);
                    }

                    break;
                }
                case "L":
                {
                    for (var i = 0; i < instruction.Distance / 90; i++)
                    {
                        waypoint = waypoint.Turn(TurnDirection.Left);
                    }

                    break;
                }
                case "N":
                    waypoint += Coordinate.North * instruction.Distance;
                    break;
                case "S":
                    waypoint += Coordinate.South * instruction.Distance;
                    break;
                case "E":
                    waypoint += Coordinate.East * instruction.Distance;
                    break;
                case "W":
                    waypoint += Coordinate.West * instruction.Distance;
                    break;
            }
        }

        var result = location.ManhattanDistance();
            
        return result.ToString();
    }
}