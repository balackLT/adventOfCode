using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2018.Day01;

public class Solution : ISolution
{
    public int Day { get; } = 1;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var result = lines.Sum(int.Parse);

        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var result = 0;
        var frequencies = new Dictionary<int, int>();

        var operations = lines.Select(int.Parse).ToList();

        while(true)
        {
            foreach(var operation in operations)
            {
                result += operation;

                if (!frequencies.ContainsKey(result))
                {
                    frequencies[result] = 1;
                }
                else
                {
                    frequencies[result]++;
                }

                if (frequencies[result] == 2)
                {
                    return result.ToString();
                }
            }
        }
    }

}