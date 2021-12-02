using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2021.Day02
{
    public class Solution : ISolution
    {
        public int Day { get; } = 2;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();

            var instructions = lines.Select(l => l.Split());

            var position = 0;
            var depth = 0;
            
            foreach (var instruction in instructions)
            {
                var x = int.Parse(instruction[1]);
                
                switch (instruction[0])
                {
                    case "down":
                        depth += x;
                        break;
                    case "up":
                        depth -= x;
                        break;
                    case "forward":
                        position += x;
                        break;
                }
            }
            
            return (position * depth).ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();

            var instructions = lines.Select(l => l.Split());

            var position = 0;
            var depth = 0;
            var aim = 0;
            
            foreach (var instruction in instructions)
            {
                var x = int.Parse(instruction[1]);
                
                switch (instruction[0])
                {
                    case "down":
                        aim += x;
                        break;
                    case "up":
                        aim -= x;
                        break;
                    case "forward":
                    {
                        position += x;
                        depth += x * aim;
                        break;
                    }
                }
            }
            
            return (position * depth).ToString();
        }

    }
}