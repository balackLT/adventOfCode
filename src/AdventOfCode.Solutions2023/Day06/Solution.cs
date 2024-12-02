using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day06;

public class Solution : ISolution
{
    public int Day { get; } = 6;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetNumbersFromLines();

        var times = lines[0].Select(int.Parse).ToList();
        var distances = lines[1].Select(int.Parse).ToList();

        var result = 1;
        for (var t = 0; t < times.Count; t++)
        {
            int time = times[t];
            var variants = 0;
            
            for (int heldFor = 1; heldFor < time; heldFor++)
            {
                var raceResult = (time - heldFor) * heldFor;
                if (raceResult > distances[t])
                    variants++;
            }
            
            result *= variants;
        }

        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetNumbersFromLines();

        var time = long.Parse(string.Concat(lines[0]));
        var distance = long.Parse(string.Concat(lines[1]));
        
        var variants = 0;

        for (int heldFor = 1; heldFor < time; heldFor++)
        {
            var raceResult = (time - heldFor) * heldFor;
            if (raceResult > distance)
                variants++;
        }

        return variants.ToString();
    }
}