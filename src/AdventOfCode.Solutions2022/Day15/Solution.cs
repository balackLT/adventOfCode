using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2022.Day15;

public class Solution : ISolution
{
    public int Day { get; } = 15;

    public object SolveFirstPart(Input input)
    {
        var sensors = input
            .GetNumbersFromLines()
            .Select(l => new Sensor(
                new Coordinate(int.Parse(l[0]), int.Parse(l[1])), 
                new Coordinate(int.Parse(l[2]), int.Parse(l[3]))
            ))
            .ToList();

        var map = new Map<char>('.');
        foreach (Sensor sensor in sensors)
        {
            map[sensor.Location] = 'S';
            map[sensor.ClosestBeacon] = 'B';
        }
        
        var targetLine = 10;
        var sensorXs = sensors.Select(s => s.Location.X);
        var sensorsInLine = sensors.Where(s => s.Location.Y == targetLine);
        var blockedCoordinates = new HashSet<Coordinate>();
        
        foreach (int x in sensorXs)
        {
            var coordinate = new Coordinate(x, targetLine);
            if (!PossibleLocation(coordinate, sensors))
            {
                blockedCoordinates.Add(coordinate);
            }
        }

        foreach (Sensor sensor in sensorsInLine)
        {
            for (var x = sensor.Location.X - sensor.Distance; x < sensor.Location.X + sensor.Distance; x++)
            {
                var coordinate = new Coordinate(x, targetLine);
                blockedCoordinates.Add(coordinate);
            }
        }

        return blockedCoordinates.Count.ToString();
    }

    private static bool PossibleLocation(Coordinate location, List<Sensor> sensors)
    {
        return sensors.All(sensor => location.ManhattanDistance(sensor.Location) > sensor.Distance);
    }

    private record Sensor(Coordinate Location, Coordinate ClosestBeacon)
    {
        public long Distance { get; init; } = Location.ManhattanDistance(ClosestBeacon);
    }
    
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        
        return 0.ToString();
    }

}