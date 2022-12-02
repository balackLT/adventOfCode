using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day20;

public class Puzzle
{
    public List<Tile> Tiles { get; init; }
    public Dictionary<Coordinate, Tile> Map { get; init; } = new ();
    public int EdgeLength { get; init; }

    public Puzzle(List<Tile> tiles)
    {
        EdgeLength = (int)Math.Sqrt(tiles.Count);
        Tiles = tiles;
    }

    public TileMap Solve()
    {
        return Solve(new List<Tile>(Tiles), new TileMap(), 0);
    }

    private TileMap Solve(List<Tile> tiles, TileMap map, int depth)
    {
        if (!tiles.Any())
        {
            map.Solved = true;
            return map;
        }

        var target = map.GetNextEmpty(EdgeLength);
        foreach (var tile in tiles)
        {
            foreach (var permutation in tile.Permutations)
            {
                if (map.TryAdd(target, permutation))
                {
                    var smallerList = new List<Tile>(tiles.Where(t => t.Number != tile.Number));
                    var solution = Solve(smallerList, map.DeepCopy(), depth + 1);
                    if (solution.Solved)
                        return solution;
                }
            }
        }
            
        // Could not finish puzzle
        return map;
    }
}