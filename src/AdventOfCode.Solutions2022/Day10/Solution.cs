using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2022.Day10;

public class Solution : ISolution
{
    public int Day { get; } = 10;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var x = 1;
        var clock = 0;
        var xValues = new Dictionary<int, int>();

        foreach (var line in lines)
        {
            var increment = 0;
            if (line.StartsWith("addx"))
            {
                increment = int.Parse(line.Split()[1]);
                xValues[clock + 1] = x;
                xValues[clock + 2] = x;
                clock += 2;
            }
            else
            {
                xValues[clock + 1] = x;
                clock++;
            }

            x += increment;
        }

        var result = xValues[20] * 20 +
                     xValues[60] * 60 +
                     xValues[100] * 100 +
                     xValues[140] * 140 +
                     xValues[180] * 180 +
                     xValues[220] * 220;

        return result.ToString();
    }
    
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var x = 1;
        var clock = 0;
        var xValues = new Dictionary<int, int>();

        foreach (var line in lines)
        {
            var increment = 0;
            if (line.StartsWith("addx"))
            {
                increment = int.Parse(line.Split()[1]);
                xValues[clock + 1] = x;
                xValues[clock + 2] = x;
                clock += 2;
            }
            else
            {
                xValues[clock + 1] = x;
                clock++;
            }

            x += increment;
        }

        xValues[0] = 1;
        var screen = new Map<char?>(null);

        for (int cycle = 0; cycle < 240; cycle++)
        {
            var sprite = new List<int>
            {
                xValues[cycle + 1] - 1,
                xValues[cycle + 1],
                xValues[cycle + 1] + 1
            };

            var row = cycle / 40;
            var position = cycle - row * 40;

            if (sprite.Contains(position))
                screen[new Coordinate(cycle, row)] = '#';
            else
                screen[new Coordinate(cycle, row)] = '.';
        }

        screen.PrintMap();

        return 0.ToString();
    }
}