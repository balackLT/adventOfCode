using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day01;

public class Solution : ISolution
{
    public int Day { get; } = 1;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLinesAsInt();

        var result = lines.Sum(CalculateFuel);

        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLinesAsInt();

        var result = 0;

        foreach (var line in lines)
        {
            var fuelMass = CalculateFuel(line);
            result += fuelMass;
            result += CalculateExtraFuel(fuelMass);
        }

        return result.ToString();
    }

    private int CalculateExtraFuel(int fuelMass)
    {
        var extraMass = CalculateFuel(fuelMass);
        var result = extraMass;

        while (true)
        {
            extraMass = CalculateFuel(extraMass);

            if (extraMass > 0)
                result += extraMass;
            else break;
        }

        return result;
    }

    private int CalculateFuel(int mass)
    {
        var result = (mass / 3) - 2;

        return result > 0 ? result : 0;
    }
}