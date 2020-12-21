using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day20
{
    public class Image
    {
        public Dictionary<Coordinate, char> Map { get; set; }
        
        public Dictionary<Coordinate, char> Monster { get; set; }
        
        public Image()
        {
            Monster = BuildMonster();
        }

        private Dictionary<Coordinate, char> BuildMonster()
        {
            var lines = new List<string>
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            var monster = new Dictionary<Coordinate, char>();

            var y = 0;
            foreach (var line in lines)
            {
                for (var x = 0; x < line.Length; x++)
                {
                    var coordinate = new Coordinate(x, y);
                    monster[coordinate] = line[x];
                }
                y++;
            }
            
            return monster;
        }

        public int CountNotMonsterTiles()
        {
            var monsterPixels = new List<List<Coordinate>>();
            
            foreach (var pixel in Map)
            {
                var window = GetWindow(pixel.Key);
                if (window is null)
                    continue;

                var monsters = GetMonsterPixels(window);
                if (monsters.Count > 0)
                    monsterPixels.Add(GetMonsterPixels(window));
            }

            var monsterCount = monsterPixels.SelectMany(l => l).Distinct().Count();
            
            if (monsterCount > 0)
                return Map.Count(c => c.Value == '#') - monsterCount;
            return 0;
        }

        private List<Coordinate> GetMonsterPixels(Dictionary<Coordinate, char> window)
        {
            var offset = window.Select(p => p.Key).First();

            var monsterCoordinates = new List<Coordinate>();
            foreach (var coordinate in Monster.Where(m => m.Value == '#'))
            {
                if (window[coordinate.Key + offset] == '#')
                    monsterCoordinates.Add(coordinate.Key + offset);
            }

            //
            // if (offset == new Coordinate(2, 2))
            // {
            //     Console.WriteLine();
            //     window.Print();
            //     Console.WriteLine();
            //     Monster.Print();
            //     Console.WriteLine();
            // }

            if (monsterCoordinates.Count == Monster.Count(m => m.Value == '#'))
                return monsterCoordinates;
            else return new List<Coordinate>();
        }

        private Dictionary<Coordinate, char> GetWindow(Coordinate coordinate)
        {
            const int monsterHeight = 3;
            const int monsterLength = 20;

            var window = new Dictionary<Coordinate, char>();
            
            for (int y = coordinate.Y; y < coordinate.Y + monsterHeight; y++)
            {
                for (int x = coordinate.X; x < coordinate.X + monsterLength; x++)
                {
                    if (Map.ContainsKey(new Coordinate(x, y)))
                        window[new Coordinate(x, y)] = Map[new Coordinate(x, y)];
                    else return null;
                }
            }

            return window;
        }

        public static Image Build(Dictionary<Coordinate, Tile> puzzle)
        {
            var map = new Dictionary<Coordinate, char>();

            var edge = puzzle.First().Value.InnerMap.Max(c => c.Key.X) + 1;
            
            foreach (var tile in puzzle)
            {
                foreach (var pixel in tile.Value.InnerMap)
                {
                    var coordinate = new Coordinate(pixel.Key.X + tile.Key.X * edge, pixel.Key.Y + tile.Key.Y * edge);
                    map[coordinate] = pixel.Value;
                }
            }

            return new Image
            {
                Map = map
            };
        }

        public void Print()
        {
            var minX = Map.Min(m => m.Key.X);
            var maxX = Map.Max(m => m.Key.X);
            var minY = Map.Min(m => m.Key.Y);
            var maxY = Map.Max(m => m.Key.Y);

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    Console.Write(Map[new Coordinate(x, y)]);
                }
                Console.WriteLine();
            }
        }
        
        private Image FlipX()
        {
            var newMap = Map.FlipX();

            var image = new Image {Map = newMap};

            return image;
        }
        
        private Image FlipY()
        {
            var newMap = Map.FlipY();
            
            var image = new Image {Map = newMap};

            return image;
        }
        
        private Image Rotate()
        {
            var newMap = Map.Rotate();

            var image = new Image {Map = newMap};

            return image;
        }
        
        public List<Image> GetPermutations()
        {
            var orientations = new List<Image>
            {
                this, 
                this.FlipX(), 
                this.FlipY(), 
                this.FlipX().FlipY()
            };

            var permutations = new List<Image>();
            foreach (var orientation in orientations)
            {
                permutations.Add(orientation);
                permutations.Add(orientation.Rotate());
                permutations.Add(orientation.Rotate().Rotate());
                permutations.Add(orientation.Rotate().Rotate().Rotate());
            }

            return permutations;
        }
    }
}