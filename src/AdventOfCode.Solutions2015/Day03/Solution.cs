using System;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2015.Day03
{
    public class Solution : ISolution
    {
        public int Day { get; } = 3;
        
        public string SolveFirstPart(Input input)
        {
            var directions = input.GetAsString();
            var map = new Map<int>(0);

            var location = new Coordinate(0, 0);
            map[location]++;
            
            foreach (var direction in directions)
            {
                var move = direction switch
                {
                    '^' => Coordinate.North,
                    '<' => Coordinate.West,
                    'v' => Coordinate.South,
                    '>' => Coordinate.East,
                    _ => throw new Exception($"Unknown direction {direction}")
                };

                location += move;
                map[location]++;
            }

            var result = map.InternalMap.Count(l => l.Value > 0);
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var directions = input.GetAsString();
            var map = new Map<int>(0);

            var santaLocation = new Coordinate(0, 0);
            map[santaLocation]++;
            
            var robotLocation = new Coordinate(0, 0);
            map[robotLocation]++;

            var santaTurn = true;
            
            foreach (var direction in directions)
            {
                var move = direction switch
                {
                    '^' => Coordinate.North,
                    '<' => Coordinate.West,
                    'v' => Coordinate.South,
                    '>' => Coordinate.East,
                    _ => throw new Exception($"Unknown direction {direction}")
                };

                if (santaTurn)
                {
                    santaLocation += move;
                    map[santaLocation]++;
                    santaTurn = false;
                }
                else
                {
                    robotLocation += move;
                    map[robotLocation]++;
                    santaTurn = true;
                }
            }

            var result = map.InternalMap.Count(l => l.Value > 0);
            
            return result.ToString();
        }
    }
}