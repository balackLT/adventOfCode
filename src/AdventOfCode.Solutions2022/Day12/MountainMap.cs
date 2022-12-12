using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2022.Day12;

public class MountainMap : Map<char>
{
    public MountainMap(char defaultLocation) : base(defaultLocation)
    {
    }

    public MountainMap(Dictionary<Coordinate, char> map, char defaultLocation) : base(map, defaultLocation)
    {
    }

    public int AStar(Coordinate start, Coordinate goal)
    {
        var openSet = new List<Coordinate> { start };
        var cameFrom = new Dictionary<Coordinate, Coordinate>();

        var gScore = new Map<int>(int.MaxValue)
        {
            [start] = 0
        };
        
        var fScore = new Map<int>(int.MaxValue)
        {
            [start] = start.ManhattanDistance(goal)
        };

        while (openSet.Count > 0)
        {
            var current = openSet.MinBy(o => fScore[o]);
            if (current == goal)
                return ReconstructPath(current, cameFrom).Count - 1;

            openSet.Remove(current!);
            
            var neighbours = current!
                .GetAdjacent()
                .Where(a => this[a] != '~')
                .Where(n => this[n] <= this[current] + 1);
            foreach (var neighbour in neighbours)
            {
                var tentativeGScore = gScore[current] + 1;
                if (tentativeGScore < gScore[neighbour])
                {
                    cameFrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = tentativeGScore + neighbour.ManhattanDistance(goal);
                    
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return int.MaxValue;
    }

    private List<Coordinate> ReconstructPath(Coordinate current, Dictionary<Coordinate, Coordinate> cameFrom)
    {
        var totalPath = new List<Coordinate> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        
        return totalPath;
    }
}