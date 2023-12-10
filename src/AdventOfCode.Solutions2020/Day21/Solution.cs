using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day21;

public class Solution : ISolution
{
    public int Day { get; } = 21;

    public object SolveFirstPart(Input input)
    {
        var groups = input
            .GetLines()
            .Select(l => l.Split("(contains "))
            .Select(line => 
                new Group
                {
                    Ingredients = line[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), 
                    Allergens = line[1].Split(',', StringSplitOptions.TrimEntries).Select(a => a.TrimEnd(')')).ToList()
                }).ToList();

        var map = SolveSudoku(groups);

        var nonAllergicFoods = groups
            .SelectMany(g => g.Ingredients)
            .Distinct()
            .Where(i => !map.ContainsKey(i))
            .ToList();

        var count = groups
            .Sum(g => nonAllergicFoods
                .Sum(nonAllergicFood => g.Ingredients.Count(i => i == nonAllergicFood)));

        return count.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var groups = input
            .GetLines()
            .Select(l => l.Split("(contains "))
            .Select(line => 
                new Group
                {
                    Ingredients = line[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList(), 
                    Allergens = line[1].Split(',', StringSplitOptions.TrimEntries).Select(a => a.TrimEnd(')')).ToList()
                }).ToList();

        var map = SolveSudoku(groups);

        var result = string.Join(',',map.OrderBy(m => m.Value).Select(m => m.Key));
            
        return result;
    }
        
    private Dictionary<string, string> SolveSudoku(List<Group> groups)
    {
        var map = new Dictionary<string, string>();
        var notFoundAllergens = new HashSet<string>(groups.SelectMany(g => g.Allergens));

        while (notFoundAllergens.Count > 0)
        {
            foreach (var allergen in notFoundAllergens)
            {
                var foundAllergens = map.Select(a => a.Key);

                var groupsWithAllergen = groups.Where(g => g.Allergens.Contains(allergen));
                var possibleFoodsWithAllergen = groupsWithAllergen
                    .Select(g => g.Ingredients)
                    .Aggregate((a, g) => a.Intersect(g).ToList())
                    .Except(foundAllergens)
                    .ToList();

                if (possibleFoodsWithAllergen.Count == 1)
                {
                    notFoundAllergens.Remove(allergen);
                    map[possibleFoodsWithAllergen.First()] = allergen;
                }
            }
        }

        return map;
    }
        
    private class Group
    {
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }
    }
}