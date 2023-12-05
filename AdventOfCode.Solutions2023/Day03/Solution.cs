using System.Collections.Frozen;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day03;

public class Solution : ISolution
{
    public int Day { get; } = 3;

    public string SolveFirstPart(Input input)
    {
        var tiles = input.GetAsCoordinateMap();

        var sum = 0L;
        
        for (int y = 0; y <= tiles.Max(t => t.Key.Y); y++)
        {
            var numberStarted = false;
            Coordinate numberStartedAt = Coordinate.Zero;
            var number = "";
            
            for (int x = 0; x <= tiles.Max(t => t.Key.X); x++)
            {
                var coordinate = new Coordinate(x, y);
                var numberEnded = false;
                var numberParsed = int.TryParse(tiles[coordinate].ToString(), out var tile);
                if (numberParsed)
                {
                    if (!numberStarted)
                    {
                        numberStarted = true;
                        numberStartedAt = coordinate;
                    }
                    number += tile;
                }

                if (numberStarted && (!numberParsed || coordinate.X == tiles.Max(t => t.Key.X)))
                {
                    numberEnded = true;
                    numberStarted = false;
                }
                
                if (numberEnded)
                {
                    var touchingSymbol = false;
                    var numberCoordinates =
                        numberStartedAt.CoordinatesStraightBetween(coordinate - new Coordinate(1, 0));
                    foreach (Coordinate numberCoordinate in numberCoordinates)
                    {
                        var adjacentCoordinates = numberCoordinate.GetAdjacentWithDiagonals();
                        foreach (Coordinate adjacentCoordinate in adjacentCoordinates)
                        {
                            if (tiles.TryGetValue(adjacentCoordinate, out var adjacentTile) && !int.TryParse(adjacentTile.ToString(), out _))
                            {
                                if (adjacentTile != '.')
                                {
                                    touchingSymbol = true;
                                }
                            }
                        }
                    }

                    if (touchingSymbol)
                    {
                        sum += long.Parse(number);
                    }
                    
                    numberStarted = false;
                    number = "";
                }
            }
        }
        
        return sum.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var tiles = input.GetAsCoordinateMap();
        
        // numbers with adjacent * and the coordinates of *
        var potentialGears = new List<(long Number, List<Coordinate> GearCoordinates)>();
        
        for (int y = 0; y <= tiles.Max(t => t.Key.Y); y++)
        {
            var numberStarted = false;
            Coordinate numberStartedAt = Coordinate.Zero;
            var number = "";
            
            for (int x = 0; x <= tiles.Max(t => t.Key.X); x++)
            {
                var coordinate = new Coordinate(x, y);
                var numberEnded = false;
                var numberParsed = int.TryParse(tiles[coordinate].ToString(), out var tile);
                if (numberParsed)
                {
                    if (!numberStarted)
                    {
                        numberStarted = true;
                        numberStartedAt = coordinate;
                    }
                    number += tile;
                }

                if (numberStarted && (!numberParsed || coordinate.X == tiles.Max(t => t.Key.X)))
                {
                    numberEnded = true;
                    numberStarted = false;
                }
                
                var gearLocationList = new List<Coordinate>();
                if (numberEnded)
                {
                    var touchingSymbol = false;
                    var numberCoordinates =
                        numberStartedAt.CoordinatesStraightBetween(coordinate - new Coordinate(1, 0));
                    foreach (Coordinate numberCoordinate in numberCoordinates)
                    {
                        var adjacentCoordinates = numberCoordinate.GetAdjacentWithDiagonals();
                        foreach (Coordinate adjacentCoordinate in adjacentCoordinates)
                        {
                            if (tiles.TryGetValue(adjacentCoordinate, out var adjacentTile) && !int.TryParse(adjacentTile.ToString(), out _))
                            {
                                if (adjacentTile == '*')
                                {
                                    touchingSymbol = true;
                                    gearLocationList.Add(adjacentCoordinate);
                                }
                            }
                        }
                    }

                    if (touchingSymbol)
                    {
                        potentialGears.Add((long.Parse(number), gearLocationList));
                    }
                    
                    numberStarted = false;
                    number = "";
                }
            }
        }
        
        var gearLocationsDictionary = new Dictionary<Coordinate, HashSet<long>>();

        foreach ((long Number, List<Coordinate> GearCoordinates) potentialGear in potentialGears)
        {
            foreach (var potentialGearGearCoordinate in potentialGear.GearCoordinates)  
            {
                if (gearLocationsDictionary.TryGetValue(potentialGearGearCoordinate, out var gearNumbers))
                {
                    gearNumbers.Add(potentialGear.Number);
                }
                else
                {
                    gearLocationsDictionary.Add(potentialGearGearCoordinate, new HashSet<long> {potentialGear.Number});
                }
            }
        }

        var result = gearLocationsDictionary
            .Where(keyValuePair => keyValuePair.Value.Count > 1)
            .Sum(keyValuePair => keyValuePair.Value.First() * keyValuePair.Value.Skip(1).First());

        return result.ToString();
    }

}