using AdventOfCode.Executor;
using MoreLinq;

namespace AdventOfCode.Solutions2023.Day05;

public class Solution : ISolution
{
    public int Day { get; } = 5;

    public record Map(string From, string To, MagicDictionary Mappings);
    
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();
        
        var seeds = lines
            .First()[7..]
            .Split()
            .Select(long.Parse);
        var maps = ParseMaps(lines).ToList();

        var lowestValue = long.MaxValue;
        foreach (long seed in seeds)
        {
            long value = TraverseSeedMaps(seed, maps);

            lowestValue = Math.Min(lowestValue, value);
        }
        
        return lowestValue.ToString();
    }

    private static long TraverseSeedMaps(long seed, List<Map> maps)
    {
        var from = "seed";
        var value = seed;

        while (true)
        {
            var map = maps.First(x => x.From == from);
            string to = map.To;

            value = map.Mappings[value];

            from = to;

            if (to == "location")
                break;
        }

        return value;
    }
    
    private static long TraverseSeedMapsInReverse(long seed, List<Map> maps)
    {
        var to = "location";
        var value = seed;

        while (true)
        {
            var map = maps.First(x => x.To == to);
            string from = map.From;

            value = map.Mappings.ReverseIndex(value);

            to = from;

            if (from == "seed")
                break;
        }

        return value;
    }

    private static IEnumerable<Map> ParseMaps(IEnumerable<string> lines)
    {
        var rawMaps = lines.Skip(2).Split("").Select(x => x.ToList());
        foreach (var rawMap in rawMaps)
        {
            var splitKind = rawMap.First().Split().First().Split('-');
            var from = splitKind[0];
            var to = splitKind[2];
            
            var mappings = new MagicDictionary();
            foreach (var row in rawMap.Skip(1))
            {
                var splitRow = row.Split();
                var destination = long.Parse(splitRow[0]);
                var source = long.Parse(splitRow[1]);
                var range = long.Parse(splitRow[2]);
                
                mappings.AddRange(destination, source, range);
            }
            
            yield return new Map(from, to, mappings);
        }
    }
    
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var parsedSeeds = lines
            .First()[7..]
            .Split()
            .Select(long.Parse)
            .ToList();

        var seedsRanges = new List<List<long>>();
        for (int i = 0; i < parsedSeeds.Count; i+= 2)
        {
            seedsRanges.Add(new List<long>{parsedSeeds[i], parsedSeeds[i + 1]});
        }

        foreach (List<long> seedsRange in seedsRanges)
        {
            Console.WriteLine($"from {seedsRange[0]} to {seedsRange[0] + seedsRange[1] - 1}");
        }
        
        var maps = ParseMaps(lines).ToList();

        var lowestValue = 0L;
        long value;
        while (true)
        {
            value = TraverseSeedMapsInReverse(lowestValue, maps);
            
            if (seedsRanges.Any(x => value >= x[0] && value <= x[0] + x[1] - 1 ))
                break;
            
            lowestValue++;
        }
        
        return lowestValue + $" seed number {value}";
    }
}