using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Executor;

public partial class Input
{
    private readonly string[] _lines;

    public Input(string[] lines)
    {
        _lines = lines;
    }

    public string[] GetLines()
    {
        return _lines;
    }
        
    public List<List<string>> GetNumbersFromLines()
    { 
        return _lines
            .Select(l => IntegersOnly().Matches(l)
                .Select(v => v.Value)
                .ToList())
            .ToList();
    }
    
    public List<List<int>> GetNumbersFromLinesAsInt()
    { 
        return _lines
            .Select(l => IntegersOnly().Matches(l)
                .Select(v => v.Value)
                .Select(int.Parse)
                .ToList())
            .ToList();
    }
    
    public List<List<long>> GetNumbersFromLinesAsLong()
    { 
        return _lines
            .Select(l => IntegersOnly().Matches(l)
                .Select(v => v.Value)
                .Select(long.Parse)
                .ToList())
            .ToList();
    }
    
    public List<List<string>> GetLinesByRegex(string regexString)
    {
        var regex = new Regex(regexString);

        return _lines
            .Select(l => regex.Match(l).Groups.Values
                .Select(v => v.Value)
                .ToList())
            .ToList();
    }
        
    public List<string> GetLinesAsList()
    {
        return _lines.ToList();
    }

    public string GetAsString()
    {
        return _lines.First();
    }

    public int[] GetLinesAsInt()
    {
        return _lines.Select(int.Parse).ToArray();
    }
        
    public int[] GetLineAsIntArray()
    {
        return _lines.First().Split(',').Select(int.Parse).ToArray();
    }

    public long[] GetLineAsLongArray()
    {
        return _lines.First().Split(',').Select(long.Parse).ToArray();
    }

    public List<List<string>> GetLinesAsLists()
    {
        return _lines.Select(line => line.Split(',').ToList()).ToList();
    }
        
    public Dictionary<(int X, int Y), char> GetAsMap()
    {
        var inputMap = GetLines();
            
        var map = new Dictionary<(int X, int Y), char>();
            
        var y = 0;
        foreach (var lines in inputMap)
        {
            var x = 0;
            foreach (var pixel in lines)
            {
                var coordinate = (x, y);
                map[coordinate] = pixel;
                    
                x++;
            }
            y++;
        }

        return map;
    }
        
    public Dictionary<Coordinate, char> GetAsCoordinateMap()
    {
        var inputMap = GetLines();
            
        var map = new Dictionary<Coordinate, char>();
            
        var y = 0;
        foreach (var lines in inputMap)
        {
            var x = 0;
            foreach (var pixel in lines)
            {
                var coordinate = new Coordinate(x, y);
                map[coordinate] = pixel;
                    
                x++;
            }
            y++;
        }

        return map;
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex IntegersOnly();
}