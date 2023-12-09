using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day17;

public class Solution : ISolution
{
    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines().Select(int.Parse).ToList();

        var permutationCount = GetPermutationCount(lines, 150);
        
        return permutationCount.ToString();
    }

    private long GetPermutationCount(List<int> buckets, int total)
    {
        if (total == 0)
        {
            return 1;
        }

        if (total < 0)
        {
            return 0;
        }

        if (buckets.Count == 0)
        {
            return 0;
        }

        var first = buckets.First();
        var rest = buckets.Skip(1).ToList();

        return GetPermutationCount(rest, total) + GetPermutationCount(rest, total - first);
    }
    
    private static List<List<int>> GeneratePermutations(List<int> buckets, int total, List<int> path)
    {
        if (total == 0)
        {
            return [path];
        }
        
        if (total < 0)
        {
            return [];
        }
        
        if (buckets.Count == 0)
        {
            return [];
        }
        
        var first = buckets.First();
        var rest = buckets.Skip(1).ToList();
        
        var permutations = GeneratePermutations(rest, total, path);
        permutations.AddRange(GeneratePermutations(rest, total - first, path.Append(first).ToList()));
        return permutations;
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines().Select(int.Parse).ToList();

        var permutations = GeneratePermutations(lines, 150, new List<int>());
        var shortestPath = permutations.Min(x => x.Count);
        var result = permutations.Count(x => x.Count == shortestPath);
        
        return result.ToString();
    }
}