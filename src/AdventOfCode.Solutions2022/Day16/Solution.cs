using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day16;

public class Solution : ISolution
{
    public int Day { get; } = 16;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var valves = ParseValves(lines);

        var start = valves["AA"];

        var result = start.CalculateMaxValue(valves, 0, 30, new List<(string, int)>());

        Console.WriteLine(result.Item2);
        
        return result.Item1.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var valves = ParseValves(lines);

        var start = valves["AA"];

        start.CalculateAllPaths(valves, 0, 30, new List<string>(), 0);

        
        
        return 0.ToString();
    }

    private static Dictionary<string, Valve> ParseValves(string[] lines)
    {
        var valves = new Dictionary<string, Valve>();

        foreach (var line in lines)
        {
            var split = line.Split();

            var name = split[1];
            var rate = int.Parse(split[4].Split('=')[1].TrimEnd(';'));
            var list = split[9..].Select(v => v.TrimEnd(','));

            Valve valve;
            if (!valves.ContainsKey(name))
            {
                valve = new Valve
                {
                    Name = name
                };
                valves[name] = valve;
            }
            else
            {
                valve = valves[name];
            }

            valve.Rate = rate;

            foreach (string neighbour in list)
            {
                if (!valves.ContainsKey(neighbour))
                {
                    var neighbourValve = new Valve
                    {
                        Name = neighbour
                    };
                    valves[neighbour] = neighbourValve;
                    valve.Neighbours.Add(neighbourValve);
                }
                else
                {
                    var neighbourValve = valves[neighbour];
                    valve.Neighbours.Add(neighbourValve);
                }
            }
        }

        foreach (var valve in valves.Values)
        {
            valve.CalculateDistances();
            valve.CleanupDistances(valves.Values.ToList());
        }

        return valves;
    }
    
    private class Valve
    {
        public string Name { get; init; }
        public int Rate { get; set; }
        public List<Valve> Neighbours { get; init; } = new();
        public Dictionary<string, int> Distances { get; init; } = new();
        public Dictionary<string, int> DistancesWithValue { get; init; } = new();
        public int Value(int depthRemaining) => Rate * depthRemaining;
        public List<(int, List<string>)> Paths = new();


        public (long, string) CalculateMaxValue(Dictionary<string, Valve> valves, int depth, int maxDepth, List<(string, int)> visited)
        {
            visited = new List<(string, int)>(visited) { (Name, depth) };
            
            if (depth >= maxDepth)
                return (0, string.Join("->", visited));

            var leftToVisit = DistancesWithValue
                .Where(d => !visited.Select(v => v.Item1).Contains(d.Key))
                .Where(d => maxDepth - depth > d.Value)
                .ToList();

            if (leftToVisit.Count == 0)
            {
                // Console.WriteLine(string.Join("->", visited));
                return ((maxDepth - depth) * Rate, string.Join("->", visited));
            }

            long bestResult = 0;
            string bestPath = string.Join("->", visited);
            foreach (var node in leftToVisit)
            {
                var valve = valves[node.Key];

                var result = valve.CalculateMaxValue(valves, depth + node.Value + 1, maxDepth, visited);
                if (result.Item1 > bestResult)
                {
                    bestResult = result.Item1;
                    bestPath = result.Item2;
                }
            }

            return (bestResult + Value(maxDepth - depth), bestPath);
        }
        
        public void CalculateAllPaths(Dictionary<string, Valve> valves, int depth, int maxDepth, List<string> visited, int value)
        {
            value += (maxDepth - depth) * Rate;
            visited = new List<string>(visited) { Name };
            
            if (depth >= maxDepth)
                Paths.Add(new ValueTuple<int, List<string>>(value, visited));

            var leftToVisit = DistancesWithValue
                .Where(d => !visited.Select(v => v).Contains(d.Key))
                .Where(d => maxDepth - depth > d.Value)
                .ToList();

            if (leftToVisit.Count == 0)
            {
                // Console.WriteLine(string.Join("->", visited));
                Paths.Add(new ValueTuple<int, List<string>>(value, visited));
            }
            
            foreach (var node in leftToVisit)
            {
                var valve = valves[node.Key];

                valve.CalculateAllPaths(valves, depth + node.Value + 1, maxDepth, visited, value);
            }
        }

        public void CalculateDistances()
        {
            var visited = new List<string>
            {
                Name
            };

            var reachable = new List<Valve>{this};
            
            for (int depth = 1; depth < 30; depth++)
            {
                reachable = reachable
                    .SelectMany(r => r.Neighbours)
                    .DistinctBy(n => n.Name)
                    .Where(n => !visited.Contains(n.Name))
                    .ToList();
                
                visited = visited.Concat(reachable.Select(r => r.Name)).Distinct().ToList();

                foreach (var valve in reachable)
                {
                    Distances[valve.Name] = depth;
                }
            }
        }

        public void CleanupDistances(List<Valve> allValves)
        {
            foreach (var distance in Distances)
            {
                if (allValves.Single(v => v.Name == distance.Key).Rate > 0)
                    DistancesWithValue[distance.Key] = distance.Value;
            }
        }
    }
}