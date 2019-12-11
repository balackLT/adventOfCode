using System;
using System.Diagnostics;
using AdventOfCode.Solutions2019.Shared.Computer;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2019.Day11
{
    public class Robot
    {
        private readonly Computer _brain;
        public Coordinate Location;
        private Coordinate _facing = Coordinate.North;

        public Robot(Computer brain, Coordinate location)
        {
            _brain = brain;
            Location = location;
        }

        public bool Finished()
        {
            return _brain.State == State.FINISHED;
        }

        public int DoStuff(int input)
        {
            _brain.Run(input);
            
            Debug.Assert(_brain.Output.Count == 2);

            var color = (int) _brain.GetOutput();
            var instruction = (int) _brain.GetOutput();

            TurnAndMove(instruction);

            return color;
        }

        private void TurnAndMove(int instruction)
        {
            var turnDirection = instruction switch
            {
                0 => TurnDirection.LEFT,
                1 => TurnDirection.RIGHT,
                _ => throw new Exception($"Invalid turn direction provided {instruction}")
            };
            
            _facing = Coordinate.Turn(_facing, turnDirection);
            Location += _facing;
        }
    }
}