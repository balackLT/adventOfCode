using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day23;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var start = map.Single(c => c.Value == '.' && c.Key.Y == 0).Key;
        var goal = map.Single(c => c.Value == '.' && c.Key.Y == map.MaxY()).Key;

        List<Path> paths = [new Path { Current = start, Visited = [start]}];

        var distances = new Dictionary<Coordinate, int>();
        foreach (var (coordinate, _) in map)
        {
            distances[coordinate] = int.MinValue;
        }
        
        while (true)
        {
            if (paths.All(p => p.Finished))
                break;
            
            List<Path> newPaths = [];
            var validPaths = paths.Where(p => !p.Finished).ToList();    
            foreach (Path path in validPaths)
            {
                List<Coordinate> adjacent = [];
                var currentSymbol = map[path.Current];
                if (currentSymbol == '.')
                {
                    adjacent = path.Current
                        .GetAdjacent()
                        .Where(c => map.TryGetValue(c, out var value) && value != '#' && !path.Visited.Contains(c))
                        .ToList();
                }
                else
                {
                    Coordinate bottom = currentSymbol switch
                    {
                        '>' => path.Current + Coordinate.Right,
                        '<' => path.Current + Coordinate.Left,
                        '^' => path.Current + Coordinate.Up,
                        'v' => path.Current + Coordinate.Down,
                        _ => throw new Exception("Unknown symbol")
                    };
                    
                    if (map.TryGetValue(bottom, out var value) && value != '#' && !path.Visited.Contains(bottom))
                        adjacent.Add(bottom);
                }
                
                if (adjacent.Count == 0)
                {
                    path.Finished = true;
                    path.DeadEnd = true;
                    continue;
                }
                
                foreach (Coordinate coordinate in adjacent)
                {
                    var depth = path.Visited.Count + 1;
                    
                    if (distances[coordinate] > depth)
                        continue;
                    
                    distances[coordinate] = depth;
                    
                    HashSet<Coordinate> newVisited = [..path.Visited, coordinate];
                    var newPath = new Path {Current = coordinate, Visited = newVisited};
                    if (coordinate == goal)
                    {
                        newPath.Finished = true;
                    }
                    
                    newPaths.Add(newPath);
                }
                
                paths.Remove(path);
            }
            
            paths = [..paths, ..newPaths];
        }

        return paths.Where(p => !p.DeadEnd).Max(p => p.Visited.Count - 1);
    }
    
    private class Path
    {
        public HashSet<Coordinate> Visited { get; init; } = [];
        public Coordinate Current { get; init; }
        public bool Finished { get; set; } = false;
        public bool DeadEnd { get; set; } = false;
        public Node? CurrentNode { get; set; } = null;
    }

    public object SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var start = map.Single(c => c.Value == '.' && c.Key.Y == 0).Key;
        var goal = map.Single(c => c.Value == '.' && c.Key.Y == map.MaxY()).Key;

        Queue<Path> paths = new();
        
        var startNode = new Node(start, new Dictionary<Node, int> ());
        paths.Enqueue(new Path { Current = start, Visited = [start], CurrentNode = startNode});
        
        while (paths.Count > 0)
        {
            var path = paths.Dequeue();
            
            if (path.Current == goal)
            {
                var finalNode = new Node(path.Current, new Dictionary<Node, int>());
                path.CurrentNode!.Distances[finalNode] = path.Visited.Count;
                continue;
            }
            
            var adjacent = path.Current
                .GetAdjacent()
                .Where(c => map.TryGetValue(c, out var value) && value != '#' && !path.Visited.Contains(c))
                .ToList();

            if (adjacent.Count == 1)
            {
                var newPath = new Path {Current = adjacent.Single(), Visited = [..path.Visited, adjacent.Single()]};
                paths.Enqueue(newPath);
                continue;
            }
            
            // crossroads
            var newNode = new Node(path.Current, new Dictionary<Node, int>());
            path.CurrentNode!.Distances[newNode] = path.Visited.Count;
            foreach (Coordinate coordinate in adjacent)
            {
                var newPath = new Path {Current = coordinate, Visited = [..path.Visited, coordinate], CurrentNode = newNode};
                paths.Enqueue(newPath);
            }
        }
        
        return 0;
    }
    
    private record Node(Coordinate Location, Dictionary<Node, int> Distances);
}
