using System;
using System.Linq;
using AdventOfCode.Solutions2019.Shared.Computer;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2019.Day17;

public class WalkWayMap
{
    public readonly Map<char> Map = new Map<char>('?');
    public readonly Computer Bot;
    public Coordinate BotLocation;
    public Coordinate BotHeading;

    public WalkWayMap(long[] botBrain)
    {
        Bot = new Computer(botBrain);
    }

    public void ConstructMap()
    {
        Bot.Run();
        var output = Bot.Output.Select(o => (char) o);

        var x = 0;
        var y = 0;
        foreach (var pixel in output)
        {
            if (pixel == 10)
            {
                y++;
                x = 0;
                continue;
            }

            Map[new Coordinate(x, y)] = pixel;
            x++;
        }

        BotLocation = Map.InternalMap
            .Single(m => (m.Value == '^' || m.Value == 'v' || m.Value == '<' || m.Value == '>')).Key;

        BotHeading = Map[BotLocation] switch
        {
            '^' => Coordinate.South, // Very specific mixup due to computer coordinates
            'v' => Coordinate.North,
            '<' => Coordinate.West,
            '>' => Coordinate.East,
            _ => throw new Exception("???")
        };
    }

    public bool IsIntersection(Coordinate coord)
    {
        if (Map[coord] != '#')
            return false;
            
        return Map[new Coordinate(coord.X, coord.Y + 1)] == '#' &&
               Map[new Coordinate(coord.X, coord.Y - 1)] == '#' &&
               Map[new Coordinate(coord.X + 1, coord.Y)] == '#' &&
               Map[new Coordinate(coord.X - 1, coord.Y)] == '#';
    }

    public int CalculateAlignmentParameter()
    {
        var keys = Map.InternalMap.Keys.ToList();
        var result = keys
            .Where(IsIntersection)
            .Sum(pixel => pixel.X * pixel.Y);

        return result;
    }

    public void Draw()
    {
        Map.PrintMap();
    }

    public string ConstructMapTraversalString()
    {
        var result = "";
        var count = 0;

        while (true)
        {
            var left = BotLocation + Coordinate.Turn(BotHeading, TurnDirection.Left);
            var right = BotLocation + Coordinate.Turn(BotHeading, TurnDirection.Right);

            if (Map[BotLocation + BotHeading] == '#')
            {
                BotLocation += BotHeading;
                count++;
            }
            else if (Map[left] == '#')
            {
                if (count > 0)
                    result += count + ",";
                BotHeading = Coordinate.Turn(BotHeading, TurnDirection.Left);
                result += "R";
                count = 0;
            }
            else if (Map[right] == '#')
            {
                if (count > 0)
                    result += count + ",";
                BotHeading = Coordinate.Turn(BotHeading, TurnDirection.Right);
                result += "L" ;
                count = 0;
            }
            else break;
        }

        return result + count;
    }
}