using AdventOfCode.Executor;
using MoreLinq.Extensions;

namespace AdventOfCode.Solutions2023.Day13;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var maps = input
            .GetLines()
            .Split("")
            .Select(l => l.ToArray())
            .ToList();
        
        var rotatedMaps = maps
            .Select(map => map
                .Transpose()
                .Select(row => new string(row.ToArray()))
                .ToArray())
            .ToList();
        
        var rowMirrorIndexes = FindMirrors(maps);
        var columnMirrorIndexes = FindMirrors(rotatedMaps);
        
        return rowMirrorIndexes.Sum() * 100 + columnMirrorIndexes.Sum();
    }

    private static List<int> FindMirrors(List<string[]> maps)
    {
        List<int> result = [];

        foreach (var map in maps)
        {
            for (int i = 1; i < map.Length; i++)
            {
                string[] firstHalf = map.Take(i).Reverse().ToArray();
                string[] secondHalf = map.Skip(i).ToArray();

                bool isMirror = true;
                for (int j = 0; j < firstHalf.Length; j++)
                {
                    if(secondHalf.Length <= j) 
                    {
                        break;
                    }
                    
                    if (firstHalf[j] != secondHalf[j])
                    {
                        isMirror = false;
                        break;
                    }
                }

                if (isMirror)
                {
                    result.Add(i);
                }
            }
        }

        return result;
    }
    
    private static List<int> FindMirrorsPartial(List<string[]> maps)
    {
        List<int> result = [];

        foreach (var map in maps)
        {
            for (int i = 1; i < map.Length; i++)
            {
                string[] firstHalf = map.Take(i).Reverse().ToArray();
                string[] secondHalf = map.Skip(i).ToArray();

                int sum = 0;
                for (int j = 0; j < firstHalf.Length; j++)
                {
                    if(secondHalf.Length <= j) 
                    {
                        break;
                    }
                    
                    for (int k = 0; k < firstHalf[j].Length; k++)
                    {
                        if (secondHalf[j][k] != firstHalf[j][k])
                        {
                            sum++;
                        }
                    }
                }

                if (sum == 1)
                {
                    result.Add(i);
                }
            }
        }

        return result;
    }
    
    public object SolveSecondPart(Input input)
    {
        var maps = input
            .GetLines()
            .Split("")
            .Select(l => l.ToArray())
            .ToList();
        
        var rotatedMaps = maps
            .Select(map => map
                .Transpose()
                .Select(row => new string(row.ToArray()))
                .ToArray())
            .ToList();
        
        var rowMirrorIndexes = FindMirrorsPartial(maps);
        var columnMirrorIndexes = FindMirrorsPartial(rotatedMaps);
        
        return rowMirrorIndexes.Sum() * 100 + columnMirrorIndexes.Sum();
    }
}