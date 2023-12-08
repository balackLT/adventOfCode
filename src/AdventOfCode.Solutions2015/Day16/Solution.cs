using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day16;

public class Solution : ISolution
{
    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var sues = new List<Sue>();
        foreach (var line in lines)
        {
            var sueNumber = int.Parse(line.Split(":")[0].Split(" ")[1]);
            var qualitiesDictionary = new Dictionary<string, int>();

            var firstColonIndex = line.IndexOf(':');
            var qualities = line[(firstColonIndex + 2)..]
                .Split(',')
                .Select(q => q.Trim());

            foreach (string quality in qualities)
            {
                var qualityParts = quality.Split(":");
                qualitiesDictionary.Add(qualityParts[0], int.Parse(qualityParts[1]));
            }

            var sue = new Sue(sueNumber, qualitiesDictionary);
            sues.Add(sue);
        }

        /*
         children: 3
        cats: 7
        samoyeds: 2
        pomeranians: 3
        akitas: 0
        vizslas: 0
        goldfish: 5
        trees: 3
        cars: 2
        perfumes: 1
         */
        for (int i = 0; i < sues.Count; i++)
        {
            if (sues[i].Qualities.TryGetValue("children", out var value) && value != 3)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("cats", out value) && value != 7)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("samoyeds", out value) && value != 2)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("pomeranians", out value) && value != 3)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("akitas", out value) && value != 0)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("vizslas", out value) && value != 0)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("goldfish", out value) && value != 5)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("trees", out value) && value != 3)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("cars", out value) && value != 2)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("perfumes", out value) && value != 1)
            {
                continue;
            }

            return (i + 1).ToString();
        }
        
        return 0.ToString();
    }
        
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var sues = new List<Sue>();
        foreach (var line in lines)
        {
            var sueNumber = int.Parse(line.Split(":")[0].Split(" ")[1]);
            var qualitiesDictionary = new Dictionary<string, int>();

            var firstColonIndex = line.IndexOf(':');
            var qualities = line[(firstColonIndex + 2)..]
                .Split(',')
                .Select(q => q.Trim());

            foreach (string quality in qualities)
            {
                var qualityParts = quality.Split(":");
                qualitiesDictionary.Add(qualityParts[0], int.Parse(qualityParts[1]));
            }

            var sue = new Sue(sueNumber, qualitiesDictionary);
            sues.Add(sue);
        }

        /*
         children: 3
        cats: 7
        samoyeds: 2
        pomeranians: 3
        akitas: 0
        vizslas: 0
        goldfish: 5
        trees: 3
        cars: 2
        perfumes: 1
         */
        for (int i = 0; i < sues.Count; i++)
        {
            if (sues[i].Qualities.TryGetValue("children", out var value) && value != 3)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("cats", out value) && value <= 7)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("samoyeds", out value) && value != 2)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("pomeranians", out value) && value >= 3)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("akitas", out value) && value != 0)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("vizslas", out value) && value != 0)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("goldfish", out value) && value >= 5)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("trees", out value) && value <= 3)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("cars", out value) && value != 2)
            {
                continue;
            }
            if (sues[i].Qualities.TryGetValue("perfumes", out value) && value != 1)
            {
                continue;
            }

            return (i + 1).ToString();
        }
        
        return 0.ToString();
    }

    private record Sue (int Number, Dictionary<string, int> Qualities);
}