using System.Collections.Frozen;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day10;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var inputMap = input.GetAsCoordinateMap();

        var map = new Dictionary<Coordinate, Tile>();
        
        foreach (KeyValuePair<Coordinate,char> keyValuePair in inputMap)
        {
            map[keyValuePair.Key] = new Tile(keyValuePair.Value, keyValuePair.Key, inputMap);
        }
        
        Dictionary<Coordinate, int> distances = TraverseMainLoop(map);

        return distances.Max(d => d.Value);
    }

    private static Dictionary<Coordinate, int> TraverseMainLoop(IDictionary<Coordinate, Tile> map)
    {
        var animalTile = map.First(x => x.Value.Symbol == 'S').Value;
        var startTiles = map
            .Where(t => t.Value.Neighbours.Contains(animalTile.Location))
            .Select(x => x.Value)
            .ToList();
        var distances = new Dictionary<Coordinate, int>();
        
        var current = startTiles;
        var depth = 1;
        foreach (Tile tile in current)
        {
            distances[tile.Location] = depth;
        }
        
        // kinda BFS
        while (true)
        {
            depth++;
            
            var newCurrent = current
                .SelectMany(c => c.Neighbours.Where(n => !distances.ContainsKey(n)))
                .Select(c => map[c])
                .ToList();

            if (newCurrent.Count == 0)
            {
                break;
            }
            
            foreach (Tile tile in newCurrent)
            {
                distances[tile.Location] = depth;
            }
            
            current = newCurrent;
        }

        return distances;
    }

    private class Tile
    {
        public Tile(char symbol, Coordinate location, Dictionary<Coordinate, char> inputMap)
        {
            Symbol = symbol;
            Location = location;
            
            var potentialNeighbours = new List<Coordinate>();
            // using inverted Y
            switch (Symbol)
            {
                case '|':
                    potentialNeighbours.Add(Location - Coordinate.North);
                    potentialNeighbours.Add(Location - Coordinate.South);
                    break;
                case '-':
                    potentialNeighbours.Add(Location + Coordinate.East);
                    potentialNeighbours.Add(Location + Coordinate.West);
                    break;
                case 'L':
                    potentialNeighbours.Add(Location - Coordinate.North);
                    potentialNeighbours.Add(Location + Coordinate.East);
                    break;
                case 'J':
                    potentialNeighbours.Add(Location - Coordinate.North);
                    potentialNeighbours.Add(Location + Coordinate.West);
                    break;
                case '7':
                    potentialNeighbours.Add(Location - Coordinate.South);
                    potentialNeighbours.Add(Location + Coordinate.West);
                    break;
                case 'F':
                    potentialNeighbours.Add(Location - Coordinate.South);
                    potentialNeighbours.Add(Location + Coordinate.East);
                    break;
            }
            
            foreach (Coordinate potentialNeighbour in potentialNeighbours.Where(inputMap.ContainsKey))
            {
                Neighbours.Add(potentialNeighbour);
            }
        }
        
        public readonly char Symbol;
        public readonly Coordinate Location;
        public readonly List<Coordinate> Neighbours = [];
    }
    
    public object SolveSecondPart(Input input)
    {
        var inputMap = input.GetAsCoordinateMap();

        IDictionary<Coordinate, Tile> map = new Dictionary<Coordinate, Tile>();
        
        foreach (KeyValuePair<Coordinate,char> keyValuePair in inputMap)
        {
            map[keyValuePair.Key] = new Tile(keyValuePair.Value, keyValuePair.Key, inputMap);
        }
        
        // so distances is not the main pipe loop
        var distances = TraverseMainLoop(map).ToFrozenDictionary();

        // we have to replace S with its actual pipe
        // I really don't to want do this programatically, so cheating it is.
        var animalTile = map.First(x => x.Value.Symbol == 'S').Value;
        var replacementForS = 'L'; // for my main input
        var newAnimalTile = new Tile(replacementForS, animalTile.Location, inputMap);
        map[animalTile.Location] = newAnimalTile;
        
        var nonPipeTileState = new Dictionary<Coordinate, TileState>();

        map = map.ToFrozenDictionary();
        
        // go south from the top and check if we're inside the loop our out
        // we only need to check within the boundaries of the main loop, everything else is outside
        for(int x = distances.Min(m => m.Key.X); 
            x <= distances.Max(m => m.Key.X); 
            x++)
        {
            var outside = true;
            var loopEdgeOpener = ' ';
            for (int y = distances.Min(m => m.Key.Y); 
                 y <= distances.Max(m => m.Key.Y); 
                 y++)
            {
                var current = map[new Coordinate(x, y)];
                
                // we're on a tile that's not part of the main loop, it's contents don't matter
                if (!distances.ContainsKey(current.Location))
                {
                    if (outside)
                    {
                        nonPipeTileState[current.Location] = TileState.FREE;
                    }
                    else
                    {
                        nonPipeTileState[current.Location] = TileState.ENCLOSED;
                    }
                }
                
                // we're crossing the main loop
                if (distances.ContainsKey(current.Location))
                {
                    // purely crossing the loop, state change
                    if (current.Symbol is '-')
                    {
                        outside = !outside;
                    }
                    
                    // we're entering the loop edge
                    if (current.Symbol is '7' or 'F')
                    {
                        loopEdgeOpener = current.Symbol;
                    }
                    
                    // we were on the edge and are now exiting
                    else if (loopEdgeOpener != ' ' && current.Symbol != '|')  
                    {
                        // or 'L' or 'J'
                        if (loopEdgeOpener == '7')
                        {
                            if (current.Symbol == 'L')
                            {
                                outside = !outside;
                            }
                        }
                        
                        if (loopEdgeOpener == 'F')
                        {
                            if (current.Symbol == 'J')
                            {
                                outside = !outside;
                            }
                        }
                            
                        loopEdgeOpener = ' ';
                    }
                }
            }
        }

        return nonPipeTileState.Count(t => t.Value == TileState.ENCLOSED);
    }
    
    private enum TileState
    {
        FREE,
        ENCLOSED
    }
}