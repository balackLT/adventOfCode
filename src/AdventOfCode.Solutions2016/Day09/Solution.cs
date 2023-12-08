using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2016.Day09;

public class Solution : ISolution
{
    public string SolveFirstPart(Input input)
    {
        var line = input.GetAsString();

        var unzipped = string.Empty;
        for(int i = 0; i < line.Length; i++)
        {
            if (line[i] == '(')
            {
                var endsAt = line.IndexOf(')', i);
                var instruction = line
                    .Substring(i + 1, endsAt - i - 1)
                    .Split('x')
                    .Select(int.Parse)
                    .ToList();
                
                var toRepeat = line.Substring(endsAt + 1, instruction[0]);
                for (int j = 0; j < instruction[1]; j++)
                {
                    unzipped += toRepeat;
                }

                i = endsAt + instruction[0];
            }
            else
            {
                unzipped += line[i];
            }
        }
        
        return unzipped.Length.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var line = input.GetAsString();

        var result = CalculateDecompressedLength(line);
        
        return result.ToString();
    }

    private static long CalculateDecompressedLength(string line)
    {
        if (line.Contains('(') == false)
        {
            return line.Length;
        }
        
        var length = 0L;
        for(int i = 0; i < line.Length; i++)
        {
            if (line[i] == '(')
            {
                var endsAt = line.IndexOf(')', i);
                var instruction = line
                    .Substring(i + 1, endsAt - i - 1)
                    .Split('x')
                    .Select(int.Parse)
                    .ToList();
                
                var toRepeat = line.Substring(endsAt + 1, instruction[0]);
                length += instruction[1] * CalculateDecompressedLength(toRepeat);

                i = endsAt + instruction[0];
            }
            else
            {
                length++;
            }
        }
        
        return length;
    }
}