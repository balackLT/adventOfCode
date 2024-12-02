using System.Collections.Frozen;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day01;

public class Solution : ISolution
{
    public int Day { get; } = 1;

    private readonly FrozenDictionary<string, int> _map = new Dictionary<string, int>
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
    }.ToFrozenDictionary();

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var total = 0;

        foreach (string line in lines)
        {
            var digits = GetDigits(line, false).ToList();
            total += GetSumOfFirstAndLastDigits(digits);
        }

        return total.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var total = 0;

        foreach (string line in lines)
        {
            var digits = GetDigits(line, true).ToList();
            total += GetSumOfFirstAndLastDigits(digits);
        }

        return total.ToString();
    }

    private int GetSumOfFirstAndLastDigits(IList<int> digits)
    {
        var number = string.Join("", new List<int>{digits.First(), digits.Last()});
      
        return int.Parse(number);
    }

    private IEnumerable<int> GetDigits(string line, bool mapNumbers)
    {
        for(int i = 0; i < line.Length; i++)
        {
            var partLine = line[i..];
            if (char.IsDigit(partLine[0]))
            {
                yield return partLine[0] - '0';
            }
            else if (mapNumbers)
            {
                foreach (KeyValuePair<string,int> keyValuePair in _map)
                {
                    if (partLine.StartsWith(keyValuePair.Key))
                    {
                        yield return keyValuePair.Value;
                    }
                }
            }
        }
    }

}