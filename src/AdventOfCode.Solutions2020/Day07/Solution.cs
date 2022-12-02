using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day07;

public class Solution : ISolution
{
    public int Day { get; } = 7;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLinesByRegex("(.+)bags contain(.+)");
            
        var bags = ParseBagsToGraph(lines);

        var gold = bags["shiny gold"];

        var result = gold.ContainedIn(bags);
            
        return result.Count.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLinesByRegex("(.+)bags contain(.+)");
            
        var bags = ParseBagsToGraph(lines);

        var gold = bags["shiny gold"];

        var result = gold.Contains();
            
        return result.ToString();
    }
        
    private Dictionary<string, Bag> ParseBagsToGraph(List<List<string>> lines)
    {
        var bags = new Dictionary<string, Bag>();

        foreach (var line in lines)
        {
            var bagName = line[1].Trim();

            if (!bags.TryGetValue(bagName, out Bag bag))
            {
                bag = new Bag
                {
                    Color = bagName
                };
                bags.Add(bagName, bag);
            }

            if (line[2].StartsWith(" no"))
                continue;

            var contents = line[2]
                .Split(',')
                .Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            var bagContains = new Dictionary<Bag, int>();
            foreach (string[] content in contents)
            {
                var key = content[1] + ' ' + content[2];
                if (!bags.TryGetValue(key, out Bag keyBag))
                {
                    keyBag = new Bag
                    {
                        Color = key
                    };
                    bags.Add(key, keyBag);
                }

                bagContains.Add(keyBag, int.Parse(content[0]));
            }

            bag.Holds = bagContains;
            bags[bag.Color] = bag;
        }

        return bags;
    }

    private class Bag
    {
        public string Color { get; init; }
        public Dictionary<Bag, int> Holds { get; set; } = new ();

        public int Contains()
        {
            var result = 0;

            foreach (var innerBag in Holds)
            {
                result += innerBag.Value;
                result += innerBag.Key.Contains() * innerBag.Value;
            }
            
            return result;
        }
            
        public HashSet<string> ContainedIn(Dictionary<string, Bag> bags)
        {
            var uniqueParents = new HashSet<string>();

            var parents = bags
                .Where(b => b.Value.Holds.ContainsKey(this))
                .ToList();

            foreach (var parent in parents)
            {
                uniqueParents.Add(parent.Key);
                foreach (var item in parent.Value.ContainedIn(bags))
                {
                    uniqueParents.Add(item);
                }
            }
            
            return uniqueParents;
        }
    }
}