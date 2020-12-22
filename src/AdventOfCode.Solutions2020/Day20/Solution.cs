using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;
using MoreLinq;

namespace AdventOfCode.Solutions2020.Day20
{
    public class Solution : ISolution
    {
        public int Day { get; } = 20;

        public string SolveFirstPart(Input input)
        {
            var tiles = input
                .GetLines()
                .Split("")
                .Select(l => Tile.ParseTile(l.ToList()))
                .ToList();

            SelfTest();
            
            var puzzle = new Puzzle(tiles);
            var solved = puzzle.Solve();
            Debug.Assert(solved.Solved);

            var cornerNW = solved.Map[Coordinate.Zero];
            var cornerNE = solved.Map[new Coordinate(puzzle.EdgeLength - 1, 0)];
            var cornerSW = solved.Map[new Coordinate(0, puzzle.EdgeLength - 1)];
            var cornerSE = solved.Map[new Coordinate(puzzle.EdgeLength - 1, puzzle.EdgeLength - 1)];

            long result = cornerNE.Number * cornerNW.Number * cornerSE.Number * cornerSW.Number;
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var tiles = input
                .GetLines()
                .Split("")
                .Select(l => Tile.ParseTile(l.ToList()))
                .ToList();

            SelfTest();
            
            var puzzle = new Puzzle(tiles);
            var solved = puzzle.Solve();
            Debug.Assert(solved.Solved);

            foreach (var tile in solved.Map)
            {
                solved.Map[tile.Key] = tile.Value.FlipX().RemoveBorders(); // turns our I worked with flipped tiles :shrug:
            }

            var image = Image.Build(solved.Map);
            // image.Print();
            // Console.WriteLine();
            // image.Monster.Print();

            var result = 0;
            foreach (var permutation in image.GetPermutations())
            {
                result = Math.Max(result, permutation.CountNotMonsterTiles());
            }
            
            return result.ToString();
        }
        
        private static void SelfTest()
        {
            var lines = new List<string>
            {
                "Tile 123:",
                "abc",
                "def",
                "ghi"
            };

            var tile = Tile.ParseTile(lines);
            var rotated = tile.Rotate();
            Debug.Assert(rotated.EastEdge == "ihg");
            Debug.Assert(rotated.SouthEdge == "adg");
            
            var flippedX = tile.FlipX();
            Debug.Assert(flippedX.EastEdge == "ifc");
            Debug.Assert(flippedX.SouthEdge == "abc");
            
            var flippedY = tile.FlipY();
            Debug.Assert(flippedY.EastEdge == "adg");
            Debug.Assert(flippedY.SouthEdge == "ihg");

            tile.GetPermutations();
            var permutations = tile.Permutations;
            Debug.Assert(tile.Permutations.Count == 16);
            Debug.Assert(tile.Permutations.All(p => p.Number == tile.Number));
            
            var lines2 = new List<string>
            {
                "Tile 123:",
                "abcd",
                "efgh",
                "ijkl",
                "mnop"
            };

            var tile2 = Tile.ParseTile(lines2);
            tile2 = tile2.RemoveBorders();
            Debug.Assert(tile2.Number == 123);
            Debug.Assert(tile2.InnerMap[new Coordinate(0, 0)] == 'f');
            Debug.Assert(tile2.InnerMap[new Coordinate(1, 0)] == 'g');
            Debug.Assert(tile2.InnerMap[new Coordinate(0, 1)] == 'j');
            Debug.Assert(tile2.InnerMap[new Coordinate(1, 1)] == 'k');
        }
    }
}