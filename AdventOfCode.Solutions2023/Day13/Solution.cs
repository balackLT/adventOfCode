using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;
using MoreLinq.Extensions;

namespace AdventOfCode.Solutions2023.Day13;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var maps = input
            .GetLines()
            .Split("")
            .Select(CoordinateDictionaryExtensions.ToMap)
            .ToList();

        return maps.Sum(map => CalculateResultForMap(map));
    }

    private static int CalculateResultForMap(Dictionary<Coordinate, char> map)
    {
        var lines = new List<string>();
        for (var y = map.MinY(); y <= map.MaxY(); y++)
        {
            var line = "";
            for (var x = map.MinX(); x <= map.MaxX(); x++)
            {
                line += map[new Coordinate(x, y)];
            }
            lines.Add(line);
        }
            
        Dictionary<int, int> lineReflections = CalculateLineReflections(lines, map);

        var columns = new List<string>();
        for (var x = map.MinX(); x <= map.MaxX(); x++)
        {
            var column = "";
            for (var y = map.MinY(); y <= map.MaxY(); y++)
            {
                column += map[new Coordinate(x, y)];
            }
            columns.Add(column);
        }
            
        Dictionary<int, int> columnReflections = CalculateColumnReflections(columns, map);

        var bestColumn = Enumerable.MaxBy(columnReflections, x => x.Value);
        var bestLine = Enumerable.MaxBy(lineReflections, x => x.Value);

        int value;
        if (bestColumn.Value > bestLine.Value)
            value = bestColumn.Key;
        else
            value = bestLine.Key * 100;
        return value;
    }

    private static Dictionary<int, int> CalculateLineReflections(List<string> lines, Dictionary<Coordinate, char> map)
    {
        var lineReflections = new Dictionary<int, int>();
        for (int i = 1; i <= lines.Count; i++)
        {
            var firstHalf = lines.Take(i).Reverse().ToList();
            var secondHalf = lines.Skip(i).ToList();

            var similarityCount = 0;
            for (int j = 0; j < map.MaxY(); j++)
            {
                if (j > firstHalf.Count - 1 || j > secondHalf.Count - 1)
                    break;
                    
                if (firstHalf[j] == secondHalf[j])
                    similarityCount++;
                else
                {
                    break;
                }
            }
            if (similarityCount == firstHalf.Count || similarityCount == secondHalf.Count)
                lineReflections[i] = similarityCount;
        }

        return lineReflections;
    }

    private static Dictionary<int, int> CalculateColumnReflections(List<string> columns, Dictionary<Coordinate, char> map)
    {
        var columnReflections = new Dictionary<int, int>();
        for (int i = 1; i <= columns.Count; i++)
        {
            var firstHalf = columns.Take(i).Reverse().ToList();
            var secondHalf = columns.Skip(i).ToList();

            var similarityCount = 0;
            for (int j = 0; j < map.MaxX(); j++)
            {
                if (j > firstHalf.Count - 1 || j > secondHalf.Count - 1)
                    break;
                    
                if (firstHalf[j] == secondHalf[j])
                    similarityCount++;
                else
                {
                    break;
                }
            }
             
            if (similarityCount == firstHalf.Count || similarityCount == secondHalf.Count)
                columnReflections[i] = similarityCount;
        }

        return columnReflections;
    }

    public object SolveSecondPart(Input input)
    {
        var maps = input
            .GetLines()
            .Split("")
            .Select(CoordinateDictionaryExtensions.ToMap)
            .ToList();

        var result = 0;
        
        foreach (var map in maps)
        {
            int value = CalculateResultForMap(map);
            foreach (var coordinate in map)
            {
                var oldChar = map[coordinate.Key];
                var newChar = oldChar == '.' ? '#' : '.';
                
                var newMap = new Dictionary<Coordinate, char>(map)
                {
                    [coordinate.Key] = newChar
                };
                
                var newValue = CalculateResultForMap(newMap);
                if (newValue > 0 && newValue != value)
                {
                    value = newValue;
                }
            }

            result += value;
        }

        return result;
    }
}