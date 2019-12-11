using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2019.Day03
{
    public class Location
    {
        public readonly Dictionary<int, int> VisitorCounts = new Dictionary<int, int>();
        public readonly Dictionary<int, int> VisitorDistances = new Dictionary<int, int>();

        public int Visit(int visitor, int distanceTraveled)
        {
            if (VisitorCounts.ContainsKey(visitor)) // This whole method is unnecesarily complicated becasue I did not understand the task...
            {
                VisitorCounts[visitor]++;

                if (VisitorDistances[visitor] < distanceTraveled)
                    return distanceTraveled;
                else
                {
                    VisitorDistances[visitor] = distanceTraveled;
                    return distanceTraveled;
                }
            }
            else
            {
                VisitorCounts[visitor] = 1;
                VisitorDistances[visitor] = distanceTraveled;
                return distanceTraveled;
            }
        }
    }
    
    public class Solution : ISolution
    {
        private Dictionary<Coordinate, Location> _map;
        
        public int Day { get; } = 3;
        
        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLinesAsLists();

            FollowWires(lines);

            var crossings = _map.Where(c => c.Value.VisitorCounts.Keys.Distinct().Count() == 2);
            var result = crossings.
                OrderBy(c => c.Key.ManhattanDistance()).
                Skip(1).FirstOrDefault().
                Key.ManhattanDistance();
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLinesAsLists();

            FollowWires(lines);

            var crossings = _map.Where(c => c.Value.VisitorCounts.Keys.Distinct().Count() == 2);
            var result = crossings.
                OrderBy(c => c.Value.VisitorDistances.Values.Sum()).
                Skip(1).FirstOrDefault().
                Value.VisitorDistances.Values.Sum();

            return result.ToString();
        }

        private void FollowWires(List<List<string>> lines)
        {
            _map = new Dictionary<Coordinate, Location>();

            var i = 0;
            foreach (var line in lines)
            {
                var coordinate = new Coordinate(0, 0);
                var distance = 0;
                
                foreach (var instruction in line)
                {
                    var move = ParseInstruction(instruction);
                    
                    distance = MoveAndDraw(coordinate, move, i, distance);
                    coordinate += move;
                }

                i++;
            }
        }

        private int MoveAndDraw(Coordinate current, Coordinate move, int visitor, int distance)
        {
            var direction = move.Normalize();
            var target = current + move;
            
            for (var location = current; location != target; location += direction)
            {
                if (!_map.ContainsKey(location))
                    _map[location] = new Location();
                
                distance = _map[location].Visit(visitor, distance) + 1;
            }

            return distance;
        }

        private Coordinate ParseInstruction(string instruction)
        {
            var direction = instruction.Substring(0, 1);
            var distance = int.Parse(instruction.Substring(1));

            var result = direction switch
            {
                "R" => new Coordinate(distance, 0),
                "L" => new Coordinate(-distance, 0),
                "U" => new Coordinate(0, distance),
                "D" => new Coordinate(0, -distance),
                _ => throw new Exception()
            };

            return result;
        }
    }
}