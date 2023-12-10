using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day15;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var ingredients = ParseIngredients(input).ToList();

        var permutations = GeneratePermutations(ingredients.Count, 100);

        var bestScore = 0;
        foreach (var permutation in permutations)
        {
            var permutationDictionary = new Dictionary<Ingredient, int>();
            for (int i = 0; i < permutation.Count; i++)
            {
                permutationDictionary.Add(ingredients[i], permutation[i]);
            }
            
            var score = TotalScore(permutationDictionary).Score;
            bestScore = Math.Max(bestScore, score);
        }
        
        return bestScore.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var ingredients = ParseIngredients(input).ToList();

        var permutations = GeneratePermutations(ingredients.Count, 100);

        var bestScore = 0;
        foreach (var permutation in permutations)
        {
            var permutationDictionary = new Dictionary<Ingredient, int>();
            for (int i = 0; i < permutation.Count; i++)
            {
                permutationDictionary.Add(ingredients[i], permutation[i]);
            }
            
            var score = TotalScore(permutationDictionary);

            if (score.Calories == 500)
            {
                bestScore = Math.Max(bestScore, score.Score);
            }
        }
        
        return bestScore.ToString();
    }

    private static List<List<int>> GeneratePermutations(int ingredients, int maxSum)
    {
        if (ingredients == 1)
        {
            return new List<List<int>> {new() {maxSum}};
        }
        
        var permutations = new List<List<int>>();
        
        for (int i = 0; i <= maxSum; i++)
        {
            var otherPermutations = GeneratePermutations(ingredients - 1, maxSum - i);
            foreach (List<int> otherPermutation in otherPermutations)
            {
                var permutation = new List<int> {i};
                permutation.AddRange(otherPermutation);
                permutations.Add(permutation);
            }
        }
        
        return permutations;
    }

    private static (int Score, int Calories) TotalScore(Dictionary<Ingredient, int> ingredients)
    {
        var capacity = 0;
        var durability = 0;
        var flavor = 0;
        var texture = 0;
        var calories = 0;
        
        foreach (var ingredient in ingredients)
        {
            capacity += ingredient.Key.Capacity * ingredient.Value;
            durability += ingredient.Key.Durability * ingredient.Value;
            flavor += ingredient.Key.Flavor * ingredient.Value;
            texture += ingredient.Key.Texture * ingredient.Value;
            calories += ingredient.Key.Calories * ingredient.Value;
        }

        if (capacity < 0) capacity = 0;
        if (durability < 0) durability = 0;
        if (flavor < 0) flavor = 0;
        if (texture < 0) texture = 0;

        var score = capacity * durability * flavor * texture;
            
        return new ValueTuple<int, int>(score, calories);
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