using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day20
{
    public class Tile
    {
        public long Number { get; init; }
        public Dictionary<Coordinate, char> InnerMap { get; init; }

        public string NorthEdge { get; set; }
        public string SouthEdge { get; set; }
        public string EastEdge { get; set; }
        public string WestEdge { get; set; }

        public List<Tile> Permutations { get; private set; }
        
        private int MaxX { get; set; }
        private int MaxY { get; set; }

        private void CalculateEdges()
        {
            MaxX = InnerMap.Max(c => c.Key.X);
            MaxY = InnerMap.Max(c => c.Key.Y);
            
            NorthEdge = new string(InnerMap.Where(c => c.Key.Y == 0).Select(c => c.Value).ToArray());
            SouthEdge = new string(InnerMap.Where(c => c.Key.Y == MaxY).Select(c => c.Value).ToArray());
            WestEdge = new string(InnerMap.Where(c => c.Key.X == 0).Select(c => c.Value).ToArray());
            EastEdge = new string(InnerMap.Where(c => c.Key.X == MaxX).Select(c => c.Value).ToArray());
        }

        public Tile RemoveBorders()
        {
            var newMap = new Dictionary<Coordinate, char>();
            
            for (int y = 1; y < MaxY; y++)
            {
                for (int x = 1; x < MaxX; x++)
                {
                    newMap[new Coordinate(x - 1, y - 1)] = InnerMap[new Coordinate(x, y)];
                }
            }
            
            var tile = new Tile
            {
                Number = this.Number,
                InnerMap = newMap
            };
            
            tile.CalculateEdges();

            return tile;
        }
        
        public void GetPermutations()
        {
            var orientations = new List<Tile>
            {
                this, 
                this.FlipX(), 
                this.FlipY(), 
                this.FlipX().FlipY()
            };

            var permutations = new List<Tile>();
            foreach (var orientation in orientations)
            {
                permutations.Add(orientation);
                permutations.Add(orientation.Rotate());
                permutations.Add(orientation.Rotate().Rotate());
                permutations.Add(orientation.Rotate().Rotate().Rotate());
            }

            Permutations = permutations;
            foreach (var tile in permutations)
            {
                tile.Permutations = permutations;
            }
        }

        public Tile Rotate()
        {
            Debug.Assert(MaxX == MaxY);
            var newMap = InnerMap.Rotate();

            var tile = new Tile
            {
                Number = this.Number,
                InnerMap = newMap
            };
            
            tile.CalculateEdges();

            return tile;
        }

        public Tile FlipX()
        {
            var newMap = InnerMap.FlipX();
            
            var tile = new Tile
            {
                Number = this.Number,
                InnerMap = newMap
            };
            
            tile.CalculateEdges();

            return tile;
        }
        
        public Tile FlipY()
        {
            var newMap = InnerMap.FlipY();
            
            var tile = new Tile
            {
                Number = this.Number,
                InnerMap = newMap
            };
            
            tile.CalculateEdges();

            return tile;
        }

        public static Tile ParseTile(List<string> input)
        {
            var innerMap = new Dictionary<Coordinate, char>();

            var y = 0;
            foreach (var line in input.Skip(1))
            {
                for (var x = 0; x < line.Length; x++)
                {
                    var coordinate = new Coordinate(x, y);
                    innerMap[coordinate] = line[x];
                }
                y++;
            }
            
            var result = new Tile
            {
                Number = int.Parse(input.First().Split()[1].TrimEnd(':')),
                InnerMap = innerMap
            };

            result.CalculateEdges();
            result.GetPermutations();

            return result;
        }
    }
}