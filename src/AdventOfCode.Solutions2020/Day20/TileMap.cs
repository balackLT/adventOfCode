using System;
using System.Collections.Generic;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day20
{
    public class TileMap
    {
        public Dictionary<Coordinate, Tile> Map { get; init; } = new ();
        public bool Solved { get; set; } = false;

        public TileMap DeepCopy()
        {
            var innerMap = new Dictionary<Coordinate, Tile>();
            foreach (var kvp in Map)
            {
                innerMap[kvp.Key] = kvp.Value;
            }

            var result = new TileMap
            {
                Map = innerMap
            };

            return result;
        }
        
        public bool TryAdd(Coordinate coordinate, Tile tile)
        {
            if (CanAdd(coordinate, tile))
            {
                Map[coordinate] = tile;
                return true;
            }

            return false;
        }

        public Coordinate GetNextEmpty(int edgeLength)
        {
            for (int y = 0; y < edgeLength; y++)
            {
                for (int x = 0; x < edgeLength; x++)
                {
                    var coordinate = new Coordinate(x, y);
                    if (!Map.ContainsKey(coordinate))
                        return coordinate;
                }
            }

            throw new Exception("We shouldn't be here :/");
        }

        private bool CanAdd(Coordinate coordinate, Tile tile)
        {
            var north = GetNeighbour(coordinate, Coordinate.North);
            if (north is not null)
            {
                if (north.SouthEdge != tile.NorthEdge)
                    return false;
            }

            var south = GetNeighbour(coordinate, Coordinate.South);
            if (south is not null)
            {
                if (south.NorthEdge != tile.SouthEdge)
                    return false;
            }

            var east = GetNeighbour(coordinate, Coordinate.East);
            if (east is not null)
            {
                if (east.WestEdge != tile.EastEdge)
                    return false;
            }

            var west = GetNeighbour(coordinate, Coordinate.West);
            if (west is not null)
            {
                if (west.EastEdge != tile.WestEdge)
                    return false;
            }

            return true;
        }

        private Tile GetNeighbour(Coordinate coordinate, Coordinate direction)
        {
            var neighbourCoordinate = coordinate + direction;

            return Map.TryGetValue(neighbourCoordinate, out var tile) ? tile : null;
        }
    }
}