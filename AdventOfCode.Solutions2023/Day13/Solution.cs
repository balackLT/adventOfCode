﻿using AdventOfCode.Executor;
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

        return maps.Sum(CalculateResultForMap);
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
            
        int lineReflections = CalculateReflections(lines, map.MaxY());

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
            
        int columnReflections = CalculateReflections(columns, map.MaxX());

        int value;
        if (columnReflections > lineReflections)
            value = columnReflections;
        else
            value = lineReflections * 100;
        return value;
    }

    private static int CalculateReflections(List<string> columns, int max)
    {
        for (int i = 1; i <= columns.Count; i++)
        {
            var firstHalf = columns.Take(i).Reverse().ToList();
            var secondHalf = columns.Skip(i).ToList();

            var similarityCount = 0;
            for (int j = 0; j < max; j++)
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

            if (similarityCount > 0 && (similarityCount == firstHalf.Count || similarityCount == secondHalf.Count))
                return i;
        }

        return 0;
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