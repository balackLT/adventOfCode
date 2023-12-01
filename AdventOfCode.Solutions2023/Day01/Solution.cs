using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day01;

public class Solution : ISolution
{
    public int Day { get; } = 1;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var total = 0;
        
        foreach (string line in lines)
        {
            var digits = line.Where(char.IsDigit).ToList();
            var number = string.Join("", new List<char>{digits.First(), digits.Last()});
            total += int.Parse(number);
        }
        
        return total.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var map = new Dictionary<string, int>()
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
        };

        var total = 0;

        foreach (string line in lines)
        {
            var digits = new List<int>();
            for(int i = 0; i < line.Length; i++)
            {
                var partLine = line[i..];
                if (char.IsDigit(partLine[0]))
                {
                    digits.Add(partLine[0] - '0');
                }
                else
                {
                    foreach (KeyValuePair<string,int> keyValuePair in map)
                    {
                        if (partLine.StartsWith(keyValuePair.Key))
                        {
                            digits.Add(keyValuePair.Value);
                            break;
                        }
                    }
                }
            }
            
            var number = string.Join("", new List<int>{digits.First(), digits.Last()});
            total += int.Parse(number);
        }
        
        return total.ToString();
    }

}