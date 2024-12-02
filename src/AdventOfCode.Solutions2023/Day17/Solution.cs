using System.Collections.Frozen;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day17;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var map = input
            .GetAsCoordinateIntMap()
            .ToFrozenDictionary();
        
        var target = new Coordinate(map.MaxX(), map.MaxY());

        var paths = new PriorityQueue<Path, long>();
        paths.Enqueue(
            new Path(Coordinate.Zero, Coordinate.Zero, 0, -1), 
            0);
        
        var shortestPath = long.MaxValue;
        var visited = new Dictionary<LocationState, long>();
        
        // weird DFS-ish, idk
        // started doing it and just went with it. Dijkstra would make more sense
        while (true)
        {
            List<Path> newPaths = [];

            if (paths.Count == 0)
                return shortestPath;

            var path = paths.Dequeue();
            
            var potentialNeighbours = path.Location.GetAdjacent();
            foreach (Coordinate neighbour in potentialNeighbours)
            {
                var steps = 1;
                
                // we cannot go backwards
                if (neighbour == path.Location - path.Direction)
                    continue;

                // continue straight if we can
                if (neighbour == path.Location + path.Direction)
                {
                    // we already went straight three times, so we cannot go straight anymore
                    if (path.Steps == 3)
                        continue;
                    
                    steps = path.Steps + 1;
                }
                
                if (!map.ContainsKey(neighbour))
                    continue;
                
                var newPath = new Path(
                    neighbour, 
                    neighbour - path.Location, 
                    path.Cost + map[neighbour], 
                    steps);
                
                if (newPath.Cost > shortestPath)
                    continue;
                
                var newState = new LocationState(newPath.Location, newPath.Direction, newPath.Steps);
                if (!visited.TryGetValue(newState, out long cost))
                {
                    visited[newState] = newPath.Cost;
                    newPaths.Add(newPath);

                    if (newPath.Location == target)
                    {
                        newPaths.Remove(newPath);
                        shortestPath = newPath.Cost;
                    }
                }
                else if (cost > newPath.Cost)
                {
                    visited[newState] = newPath.Cost;
                    newPaths.Add(newPath);

                    if (newPath.Location == target)
                    {
                        newPaths.Remove(newPath);
                        shortestPath = newPath.Cost;
                    }
                }
                
            }

            foreach (Path newPath in newPaths)
            {
                paths.Enqueue(newPath, newPath.Cost + newPath.Location.ManhattanDistance(target));
            }
        }
    }

    private record LocationState(Coordinate Location, Coordinate Direction, int Steps);
    private record Path(Coordinate Location, Coordinate Direction, long Cost, int Steps, List<Coordinate>? PathTraveled = null);

    public object SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateIntMap().ToFrozenDictionary();
        
        var target = new Coordinate(map.MaxX(), map.MaxY());

        var paths = new PriorityQueue<Path, long>();
        paths.Enqueue(
            new Path(Coordinate.Zero, Coordinate.Zero, 0, 100, []), 
            0);
        
        var shortestPath = long.MaxValue;
        var visited = new Dictionary<LocationState, long>();
        
        // weird DFS-ish, idk
        // started doing it and just went with it. Dijkstra would make more sense
        while (true)
        {
            List<Path> newPaths = [];

            if (paths.Count == 0)
                return shortestPath;

            var path = paths.Dequeue();
            
            List<Coordinate> potentialNeighbours = path.Location.GetAdjacent().ToList();
            foreach (Coordinate potentialNeighbour in potentialNeighbours)
            {
                Coordinate neighbour = potentialNeighbour;

                // we cannot go backwards
                if (neighbour == path.Location - path.Direction)
                    continue;

                if (!map.ContainsKey(neighbour))
                    continue;
                
                var newCost = path.Cost + map[neighbour];

                Path newPath;
                
                // continue straight if we can
                if (neighbour == path.Location + path.Direction)
                {
                    // we already went straight 10 times, so we cannot go straight anymore
                    if (path.Steps == 10)
                        continue;

                    // we must go straight at least 4 times before turning
                    int steps;
                    if (path.Steps == 1)
                    {
                        neighbour = path.Location + path.Direction * 4;
                        steps = 4;

                        newCost = path.Cost;
                        foreach (Coordinate coordinate in path.Location.CoordinatesStraightBetween(neighbour).Skip(1))
                        {
                            if (!map.ContainsKey(coordinate))
                                continue;

                            newCost += map[coordinate];
                        }
                    }
                    else
                    {
                        steps = path.Steps + 1;
                    }
                    
                    newPath = new Path(
                        neighbour,
                        (neighbour - path.Location).Normalize(),
                        newCost,
                        steps,
                        [path.Location, ..path.PathTraveled]);
                }
                else if (path.Steps < 4 && path.Steps > 0)
                {
                    // we must go straight at least 4 times before turning
                    continue;   
                }
                else
                {
                    // change direction only
                    newPath = new Path(
                        path.Location,
                        (neighbour - path.Location).Normalize(),
                        path.Cost,
                        1,
                        [..path.PathTraveled]);
                }

                if (!map.ContainsKey(neighbour))
                    continue;

                if (newPath.Cost > shortestPath)
                    continue;

                var newState = new LocationState(newPath.Location, newPath.Direction, newPath.Steps);
                if (!visited.TryGetValue(newState, out long cost))
                {
                    visited[newState] = newPath.Cost;
                    newPaths.Add(newPath);

                    if (newPath.Location == target)
                    {
                        shortestPath = newPath.Cost;
                        newPaths.Remove(newPath);
                        // newPath.PathTraveled!.Reverse();
                        // newPath.PathTraveled.Add(newPath.Location);
                        // Console.WriteLine(string.Join("->", newPath.PathTraveled));
                    }
                        
                }
                else if (cost > newPath.Cost)
                {
                    visited[newState] = newPath.Cost;
                    newPaths.Add(newPath);

                    if (newPath.Location == target)
                    {
                        shortestPath = newPath.Cost;
                        newPaths.Remove(newPath);
                    }
                }
            }

            foreach (Path newPath in newPaths)
            {
                paths.Enqueue(newPath, newPath.Cost + newPath.Location.ManhattanDistance(target));
            }
        }
    }
}