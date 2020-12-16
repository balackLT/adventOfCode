using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2018.Day03
{
    public class Solution : ISolution
    {
        public int Day { get; } = 3;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();

            var instructions = new List<Instruction>();

            foreach (var line in lines)
            {
                var instruction = new Instruction();

                var atLocation = line.IndexOf('@');
                var commaLocation = line.IndexOf(',');
                var colonLocation = line.IndexOf(':');
                var xLocation = line.IndexOf('x');

                instruction.Id = int.Parse(line.Substring(1, atLocation - 1));
                instruction.X = int.Parse(line.Substring(atLocation + 1, commaLocation - atLocation - 1));
                instruction.Y = int.Parse(line.Substring(commaLocation + 1, colonLocation - commaLocation - 1));
                instruction.Length = int.Parse(line.Substring(colonLocation + 1, xLocation - colonLocation - 1));
                instruction.Height = int.Parse(line.Substring(xLocation + 1));
                instruction.isOverlapped = false;

                instructions.Add(instruction);
            }

            var santasFabric = new Fabric();

            santasFabric.Fill(instructions);

            return santasFabric.CountOverlap().ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();

            var instructions = new List<Instruction>();

            foreach (var line in lines)
            {
                var instruction = new Instruction();

                var atLocation = line.IndexOf('@');
                var commaLocation = line.IndexOf(',');
                var colonLocation = line.IndexOf(':');
                var xLocation = line.IndexOf('x');

                instruction.Id = int.Parse(line.Substring(1, atLocation - 1));
                instruction.X = int.Parse(line.Substring(atLocation + 1, commaLocation - atLocation - 1));
                instruction.Y = int.Parse(line.Substring(commaLocation + 1, colonLocation - commaLocation - 1));
                instruction.Length = int.Parse(line.Substring(colonLocation + 1, xLocation - colonLocation - 1));
                instruction.Height = int.Parse(line.Substring(xLocation + 1));
                instruction.isOverlapped = false;

                instructions.Add(instruction);
            }

            var santasFabric = new Fabric();

            santasFabric.Fill(instructions);

            return instructions.FirstOrDefault(i => i.isOverlapped == false)?.Id.ToString();
        }
        
        class Coordinate
    {
        public int Value {get; set;}
        public int ClaimId {get; set;}
    }

    class Instruction
    {
        public int Id {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
        public int Length {get; set;}
        public int Height {get; set;}
        public bool isOverlapped {get; set;}
    }

    class Fabric
    {
        public Coordinate[,] Grid {get; set;} = new Coordinate[1000, 1000];

        public Fabric()
        {
            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    Grid[x,y] = new Coordinate {
                        Value = 0,
                        ClaimId = 0
                    };
                }
            }
        }

        public void Fill (List<Instruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                for (int y = instruction.X; y < instruction.X + instruction.Length; y++)
                {
                    for (int x = instruction.Y; x < instruction.Y + instruction.Height; x++)
                    {
                        Grid[x,y].Value++;

                        if (Grid[x,y].ClaimId == 0)
                        {
                            Grid[x,y].ClaimId = instruction.Id;
                        }
                        else
                        {
                            instructions[Grid[x,y].ClaimId - 1].isOverlapped = true;
                            instruction.isOverlapped = true;
                        }
                    }
                }
            }
        }

        public int CountOverlap ()
        {
            var result = 0;

            for (int y = 0; y < 1000; y++)
            {
                for (int x = 0; x < 1000; x++)
                {
                    if (Grid[x,y].Value > 1)
                        result++;
                }
            }

            return result;
        }
    }

    }
}