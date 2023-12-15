using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day15;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var sequence = input
            .GetLines().SelectMany(l => l.Split(','));

        return sequence
            .Select(CalculateHash)
            .Aggregate(0L, (current, value) => current + value);
    }

    private static int CalculateHash(string s)
    {
        var value = 0;
        foreach (char c in s)
        {
            value += c;
            value *= 17;
            value %= 256;
        }

        return value;
    }

    public object SolveSecondPart(Input input)
    {
        var sequence = input
            .GetLines().SelectMany(l => l.Split(','));

        var boxes = new List<List<Lens>>();
        for (int i = 0; i < 256; i++)
        {
            boxes.Add([]);
        }

        foreach (string instruction in sequence)
        {
            if (instruction.Contains('='))
            {
                var split = instruction.Split('=');
                var lens = new Lens(split[0], int.Parse(split[1]));
                var hash = CalculateHash(lens.Name);
                
                var box = boxes[hash];
                var lensIndex = box.FindIndex(l => l.Name == lens.Name);
                if (lensIndex != -1)
                {
                    box[lensIndex] = lens;
                }
                else
                {
                    box.Add(lens);
                }
            }
            else
            {
                var name = instruction[..^1];
                var hash = CalculateHash(name);
                
                var box = boxes[hash];
                var lensIndex = box.FindIndex(l => l.Name == name);
                if (lensIndex != -1)
                {
                    box.RemoveAt(lensIndex);
                }
            }
        }

        var result = 0L;
        for (int boxIndex = 0; boxIndex < boxes.Count; boxIndex++)
        {
            for (int lensIndex = 0; lensIndex < boxes[boxIndex].Count; lensIndex++)
            {
                long value = (1 + boxIndex) * (1 +lensIndex) * boxes[boxIndex][lensIndex].FocalLength;
                result += value;
            }
        }
        
        return result;
    }

    private record Lens(string Name, int FocalLength);
}