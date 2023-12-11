using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day11;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var inputMap = input.GetAsCoordinateMap();

        List<Galaxy> galaxies = ParseGalaxies(inputMap, 2);

        long result = DistancesBetweenAllGalaxyPairs(galaxies);

        return result;
    }

    private static long DistancesBetweenAllGalaxyPairs(List<Galaxy> galaxies)
    {
        var result = 0L;

        var galaxyStack = new Stack<Galaxy>(galaxies);
        while (galaxyStack.Count != 0)
        {
            var galaxyToCheck = galaxyStack.Pop();
            foreach (Galaxy galaxy in galaxyStack)
            {
                var distance = galaxyToCheck.TrueLocation.ManhattanDistance(galaxy.TrueLocation);
                result += distance;
            }
        }

        return result;
    }

    private static List<Galaxy> ParseGalaxies(Dictionary<Coordinate, char> inputMap, int redShift)
    {
        var galaxies = new List<Galaxy>();

        var shift = 0;
        for (int y = 0; y <= inputMap.MaxY(); y++)
        {
            var column = inputMap
                .Where(m => m.Key.Y == y)
                .ToList();
            
            if (column.All(c => c.Value == '.'))
            {
                shift += redShift - 1;
            }
            else
            {
                var columnGalaxies = column
                    .Where(c => c.Value == '#')
                    .ToList();

                galaxies.AddRange(columnGalaxies
                    .Select(galaxy => new Galaxy
                    {
                        Location = galaxy.Key, ShiftX = 0, ShiftY = shift
                    }));
            }
        }

        shift = 0;
        for(int x = 0; x <= inputMap.MaxX(); x++)
        {
            var row = inputMap
                .Where(m => m.Key.X == x)
                .ToList();
            
            if (row.All(c => c.Value == '.'))
            {
                shift += redShift - 1;
            }
            else
            {
                var rowGalaxies = row
                    .Where(c => c.Value == '#')
                    .ToList();

                foreach (var rowGalaxy in rowGalaxies)
                {
                    var galaxy = galaxies
                        .First(g => g.Location.X == rowGalaxy.Key.X && g.Location.Y == rowGalaxy.Key.Y);
                    
                    galaxy.ShiftX = shift;
                }
            }
        }

        return galaxies;
    }

    private class Galaxy
    {
        public Coordinate Location;
        public int ShiftX;
        public int ShiftY;
        
        public Coordinate TrueLocation => new(Location.X + ShiftX, Location.Y + ShiftY);
    }

    public object SolveSecondPart(Input input)
    {
        var inputMap = input.GetAsCoordinateMap();

        List<Galaxy> galaxies = ParseGalaxies(inputMap, 1000000);

        long result = DistancesBetweenAllGalaxyPairs(galaxies);
        
        return result;
    }
}