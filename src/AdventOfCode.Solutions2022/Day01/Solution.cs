using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day01
{
    public class Solution : ISolution
    {
        public int Day { get; } = 1;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();

            var elves = new List<int>();
            var elf = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(elf);
                    elf = 0;
                }
                else
                {
                    elf += int.Parse(line);
                }
            }
            
            return elves.Max().ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();

            var elves = new List<int>();
            var elf = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(elf);
                    elf = 0;
                }
                else
                {
                    elf += int.Parse(line);
                }
            }

            return elves.OrderDescending().Take(3).Sum().ToString();
        }

    }
}