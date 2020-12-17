using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day09
{
    public class Solution : ISolution
    {
        public int Day { get; } = 9;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLinesByRegex(@"(\w+) to (\w+) = (\d+)");

            var locations = ParseLocations(lines);

            var shortestPath = locations.Min(l => l.TraverseAllConnections(new List<Location>(), 0));
            
            return shortestPath.ToString();
        }

        
        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLinesByRegex(@"(\w+) to (\w+) = (\d+)");

            var locations = ParseLocations(lines);

            var shortestPath = locations.Max(l => l.TraverseAllConnectionsSlowly(new List<Location>(), 0));
            
            return shortestPath.ToString();
        }

        private static List<Location> ParseLocations(List<List<string>> lines)
        {
            var locations = new List<Location>();

            foreach (var line in lines)
            {
                var from = line[1];
                var to = line[2];
                var distance = int.Parse(line[3]);

                Location fromLocation;
                Location toLocation;

                if (locations.Any(l => l.Name == from))
                {
                    fromLocation = locations.First(l => l.Name == from);
                }
                else
                {
                    fromLocation = new Location
                    {
                        Name = from
                    };
                    locations.Add(fromLocation);
                }

                if (locations.Any(l => l.Name == to))
                {
                    toLocation = locations.First(l => l.Name == to);
                }
                else
                {
                    toLocation = new Location
                    {
                        Name = to
                    };
                    locations.Add(toLocation);
                }

                fromLocation.Distances[toLocation] = distance;
                toLocation.Distances[fromLocation] = distance;
            }

            return locations;
        }
        
        private record Location
        {
            public string Name { get; init; }
            public Dictionary<Location, int> Distances { get; } = new();
            
            public int TraverseAllConnections(List<Location> locationsTraversed, int distanceTraveled)
            {
                locationsTraversed.Add(this);

                var notVisited = Distances.Where(d => locationsTraversed.All(l => l.Name != d.Key.Name)).ToList();
                if (!notVisited.Any())
                    return distanceTraveled;

                return notVisited
                    .Select(location => location.Key
                        .TraverseAllConnections(new List<Location>(locationsTraversed), distanceTraveled + location.Value))
                    .Prepend(int.MaxValue)
                    .Min();
            }
            
            public int TraverseAllConnectionsSlowly(List<Location> locationsTraversed, int distanceTraveled)
            {
                locationsTraversed.Add(this);

                var notVisited = Distances.Where(d => locationsTraversed.All(l => l.Name != d.Key.Name)).ToList();
                if (!notVisited.Any())
                    return distanceTraveled;

                return notVisited
                    .Select(location => location.Key.TraverseAllConnectionsSlowly(new List<Location>(locationsTraversed), distanceTraveled + location.Value))
                    .Prepend(int.MinValue)
                    .Max();
            }
        }
    }
}