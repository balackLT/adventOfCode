using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day15;

public class Solution : ISolution
{
    public int Day { get; } = 15;

    public string SolveFirstPart(Input input)
    {
        var ingredients = ParseIngredients(input).ToList();

            

        return 0.ToString();
    }
        
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();
            
            
        return 0.ToString();
    }

    private int TotalScore(IEnumerable<(Ingredient, int)> ingredients)
    {
        var capacity = 0;
        var durability = 0;
        var flavor = 0;
        var texture = 0;

        foreach (var ingredient in ingredients)
        {
            capacity += ingredient.Item1.Capacity * ingredient.Item2;
            durability += ingredient.Item1.Durability * ingredient.Item2;
            flavor += ingredient.Item1.Flavor * ingredient.Item2;
            texture += ingredient.Item1.Texture * ingredient.Item2;
        }

        if (capacity < 0) capacity = 0;
        if (durability < 0) durability = 0;
        if (flavor < 0) flavor = 0;
        if (texture < 0) texture = 0;

        var score = capacity * durability * flavor * texture;
            
        return score;
    }
        
    private static IEnumerable<Ingredient> ParseIngredients(Input input)
    {
        var ingredients = input.GetLines()
            .Select(l => l.Split(":"))
            .Select(l => new
            {
                Name = l[0],
                Stuff = l[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(e => e.TrimEnd(',')).ToList()
            })
            .Select(l => new Ingredient(
                l.Name,
                int.Parse(l.Stuff[1]),
                int.Parse(l.Stuff[3]),
                int.Parse(l.Stuff[5]),
                int.Parse(l.Stuff[7]),
                int.Parse(l.Stuff[9])));
        return ingredients;
    }

    private record Ingredient(string Name, int Capacity, int Durability, int Flavor, int Texture, int Calories);
}