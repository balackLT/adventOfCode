using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day06;

public class Solution : ISolution
{
    public int Day { get; } = 6;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLinesAsList();

        var groups = ExtractGroups(lines);

        var result = groups
            .Select(g => g
                .SelectMany(a => a)
                .Distinct()
                .Count())
            .Sum();
            
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLinesAsList();

        var groups = ExtractGroups(lines);

        var result = groups
            .Select(g => g.Distinct())
            .Select(g => new
            {
                PeopleCount = g.Count(), 
                Answers = g
                    .SelectMany(a => a)
                    .GroupBy(s => s)
                    .Select(l => new {Value = l.Key, Count = l.Count()})
            })
            .Select(g => g.Answers.Count(a => a.Count == g.PeopleCount))
            .Sum();
            
        return result.ToString();
    }
        
    private List<List<string>> ExtractGroups(List<string> lines)
    {
        var groups = new List<List<string>>();
        var group = new List<string>();
        foreach (var answerLine in lines)
        {
            if (answerLine.Length > 0)
            {
                group.Add(answerLine);
            }
            else
            {
                groups.Add(group);
                group = new List<string>();
            }
        }

        groups.Add(group);
        return groups;
    }
}