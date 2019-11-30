using System.Collections.Generic;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions._2018.day01
{
    public class Solution : ISolution
    {
        public int Day { get; } = 1;

        public string SolveFirstPart(Input input)
        {
            string[] lines = input.GetLines();

            var result = 0;

            foreach (string line in lines)
            {
                if (line[0] == '+')
                {
                    result += int.Parse(line.Substring(1));
                }
                else
                {
                    result -= int.Parse(line.Substring(1));
                }
            }

            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            string[] lines = input.GetLines();

            var result = 0;
            var operations = new List<int>();
            var frequencies = new Dictionary<int, int>();

            foreach (string line in lines)
            {
                operations.Add(int.Parse(line));
            }

            int loops = 0;
            while(true)
            {
                loops++;
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
                        return $"{result}, {loops}";
                    }
                }
            }
        }
    }
}
