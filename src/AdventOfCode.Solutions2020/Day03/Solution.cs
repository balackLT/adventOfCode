using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day03
{
    public class Solution : ISolution
    {
        public int Day { get; } = 3;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetAsMap();

            var map = new MountainMap(lines);

            var result = CalcTrees(map, new Coordinate(3, 1));
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetAsMap();

            var map = new MountainMap(lines);

            var result = CalcTrees(map, new Coordinate(1, 1));
            result *= CalcTrees(map, new Coordinate(3, 1));
            result *= CalcTrees(map, new Coordinate(5, 1));
            result *= CalcTrees(map, new Coordinate(7, 1));
            result *= CalcTrees(map, new Coordinate(1, 2));
            
            return result.ToString();
        }

        private int CalcTrees(MountainMap map, Coordinate velocity)
        {
            var position = Coordinate.Zero;
            var trees = 0;

            while (true)
            {
                // Console.WriteLine($"{position}: {map[position]}");
                
                if (map[position] == '#')
                    trees++;

                position += velocity;
                
                if (position.Y > map.Height)
                    break;
            }

            return trees;
        }
        
        private class MountainMap
        {
            private readonly Dictionary<Coordinate, char> _map = new();
            public int Width { get; }
            public int Height { get; }

            public MountainMap(Dictionary<(int X, int Y), char> map)
            {
                foreach (((int X, int Y) key, char value) in map)
                {
                    _map[new Coordinate(key.X, key.Y)] = value;
                }

                Width = _map.Max(c => c.Key.X);
                Height = _map.Max(c => c.Key.Y);
            }
            
            public char this[Coordinate index]   
            {
                get
                {
                    while (index.X > Width)
                    {
                        index.X -= Width + 1;
                    }

                    return _map[index];
                }
            }
        }

    }
}