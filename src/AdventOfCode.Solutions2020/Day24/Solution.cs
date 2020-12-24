using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using Sprache;
using Input = AdventOfCode.Executor.Input;

namespace AdventOfCode.Solutions2020.Day24
{
    public class Solution : ISolution
    {
        private static readonly Parser<Hex> East = Parse.String("e").Select(h => Hex.East);
        private static readonly Parser<Hex> SouthEast = Parse.String("se").Select(h => Hex.SouthEast);
        private static readonly Parser<Hex> NorthEast = Parse.String("ne").Select(h => Hex.NorthEast);
        private static readonly Parser<Hex> West = Parse.String("w").Select(h => Hex.West);
        private static readonly Parser<Hex> SouthWest = Parse.String("sw").Select(h => Hex.SouthWest);
        private static readonly Parser<Hex> NorthWest = Parse.String("nw").Select(h => Hex.NorthWest);

        private static readonly Parser<IEnumerable<Hex>> Hexes = East
            .Or(SouthEast)
            .Or(NorthEast)
            .Or(West)
            .Or(SouthWest)
            .Or(NorthWest)
            .Many();
        
        public int Day { get; } = 24;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLines();

            var moves = lines.Select(l => Hexes.Parse(l).ToList()).ToList();

            var map = new HexMap();

            foreach (var move in moves)
            {
                var current = new Hex(0, 0);
                foreach (var step in move)
                {
                    current += step;
                }

                var value = map[current];
                map[current] = !value;
            }

            var result = map.Map.Count(h => h.Value);
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLines();

            var moves = lines.Select(l => Hexes.Parse(l).ToList()).ToList();

            var map = new HexMap();

            foreach (var move in moves)
            {
                var current = new Hex(0, 0);
                foreach (var step in move)
                {
                    current += step;
                }

                var value = map[current];
                map[current] = !value;
            }

            for (int i = 0; i < 100; i++)
            {
                map.ConwayStuff();
            }

            var result = map.Map.Count(h => h.Value);
            
            return result.ToString();
        }

        private class HexMap
        {
            public readonly Dictionary<Hex, bool> Map = new();

            public void ConwayStuff()
            {
                var maxX = Map.Max(h => h.Key.X);
                var maxY = Map.Max(h => h.Key.Y);
                
                var minX = Map.Min(h => h.Key.X);
                var minY = Map.Min(h => h.Key.Y);

                var hexesToFlip = new List<Hex>();
                
                for (int y = minY - 1; y <= maxY + 1; y++)
                {
                    for (int x = minX - 1; x <= maxX + 1; x++)
                    {
                        var hex = new Hex(x, y);

                        var blackFriends = hex.Friends().Count(f => this[f]);
                        
                        if (this[hex] == true && (blackFriends > 2 || blackFriends == 0))
                            hexesToFlip.Add(hex);
                        
                        if (this[hex] == false && blackFriends == 2)
                            hexesToFlip.Add(hex);
                    }
                }

                foreach (var hex in hexesToFlip)
                {
                    Map[hex] = !Map[hex];
                }
            }
            
            public bool this[Hex index]   
            {
                get
                {
                    if (Map.TryGetValue(index, out var value))
                        return value;

                    Map[index] = false;
                
                    return false;
                }
                set => Map[index] = value;
            }
        }
        
        private record Hex(int X, int Y)
        {
            public static readonly Hex East = new Hex(1, 0);
            public static readonly Hex SouthEast = new Hex(0, -1);
            public static readonly Hex NorthEast = new Hex(1, 1);
            
            public static readonly Hex West = new Hex(-1, 0);
            public static readonly Hex NorthWest = new Hex(0, 1);
            public static readonly Hex SouthWest = new Hex(-1, -1);

            public IEnumerable<Hex> Friends()
            {
                yield return this + East;
                yield return this + SouthEast;
                yield return this + NorthEast;
                yield return this + West;
                yield return this + NorthWest;
                yield return this + SouthWest;
            }
            
            public static Hex operator +(Hex left, Hex right)
            {
                return new(left.X + right.X, left.Y + right.Y);
            }
        }
    }
}