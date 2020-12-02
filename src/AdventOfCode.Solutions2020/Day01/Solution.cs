using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day01
{
    public class Solution : ISolution
    {
        public int Day { get; } = 1;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLinesAsInt();

            foreach (var line in lines)    
            {
                foreach (var line2 in lines)    
                {
                    if (line + line2 == 2020)
                    {
                        return (line * line2).ToString();
                    }
                }
            }

            return 0.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLinesAsInt();

            foreach (var line in lines)    
            {
                foreach (var line2 in lines)    
                {
                    foreach (var line3 in lines)    
                    {
                        if (line + line2 + line3 == 2020)
                        {
                            return (line * line2 * line3).ToString();
                        }
                    }
                }
            }

            return 0.ToString();
        }

    }
}