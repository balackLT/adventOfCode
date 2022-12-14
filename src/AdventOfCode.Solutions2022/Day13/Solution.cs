using System.Text.Json.Nodes;
using AdventOfCode.Executor;
using MoreLinq;

namespace AdventOfCode.Solutions2022.Day13;

public class Solution : ISolution
{
    public int Day { get; } = 13;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var pairs = lines
            .Split("")
            .Select(p => p
                .Select(m => JsonNode.Parse(m) as JsonArray)
                .ToList())
            .Select(p => new ArrayPair{Left = p[0]!, Right = p[1]!})
            .ToList();

        var result = 0;
        
        var i = 1;
        foreach (var pair in pairs)
        {
            if (pair.Ordered)
                result += i;
            i++;
            Console.WriteLine();
        }
        
        return result.ToString();
    }
    
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines().ToList();

        lines.Add("[[2]]");
        lines.Add("[[6]]");
            
        var packets = lines    
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l => JsonNode.Parse(l) as JsonArray)
            .ToList();

        var orderedPackets = packets
            .OrderBy(p => p, new JArrayComparer()!)
            .ToList();

        var result = 1;

        var i = 1;
        foreach (var packet in orderedPackets)
        {
            if (packet!.ToJsonString() == "[[2]]" || packet.ToJsonString() == "[[6]]")
                result *= i;

            i++;
        }
        
        return result.ToString();
    }

    private class JArrayComparer : IComparer<JsonArray>
    {
        public int Compare(JsonArray? left, JsonArray? right)
        {
            var result = ArrayPair.Compare(left!, right!);
            
            if (result == ArrayPair.ComparisonResult.Equal) return 0;
            if (result == ArrayPair.ComparisonResult.Disordered) return 1;
            if (result == ArrayPair.ComparisonResult.Ordered) return -1;

            throw new Exception("what");
        }
    }
}