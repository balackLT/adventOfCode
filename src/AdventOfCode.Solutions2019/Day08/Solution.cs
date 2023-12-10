using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day08;

public class Solution : ISolution
{
    public int Day { get; } = 8;
        
    public object SolveFirstPart(Input input)
    {
        var pixels = input.GetAsString().Select(c => int.Parse(c.ToString())).ToList();

        var layers = ConvertToLayers(pixels);
            
        var resultLayer = layers.OrderBy(l => l.Count(p => p == 0)).First();
        var result = resultLayer.Count(p => p == 1) * resultLayer.Count(p => p == 2);
            
        return result.ToString();
    }

    private List<List<int>> ConvertToLayers(List<int> pixels)
    {
        var layersPixels = new List<List<int>>();

        var iterator = 0;
        while (true)
        {
            if (pixels.Count <= iterator)
                break;
                
            var layer = new List<int>();

            for (int i = 0; i < 150; i++)
            {
                layer.Add(pixels[iterator]);
                iterator++;
            }
                
            layersPixels.Add(layer);
        }

        return layersPixels;
    }

    public object SolveSecondPart(Input input)
    {
        var pixels = input.GetAsString().Select(c => int.Parse(c.ToString())).ToList();

        var layers = ConvertToLayers(pixels);
        var mergedLayers = MergeLayers(layers);

        Render(mergedLayers);

        return 0.ToString();
    }

    private void Render(List<int> mergedLayers)
    {
        var count = 0;
        foreach (var pixel in mergedLayers)
        {
            if (pixel == 0)
                Console.Write("█");
            else Console.Write(" ");

            count++;
            if (count % 25 == 0)
                Console.WriteLine();
        }
    }

    private List<int> MergeLayers(List<List<int>> layers)
    {
        var merged = new List<int>(Enumerable.Repeat(2, 150)); // Transparent layer

        layers.Reverse();
        foreach (var layer in layers)
        {
            for (var i = 0; i < 150; i++)
            {
                if (layer[i] == 0)
                    merged[i] = 0;
                    
                if (layer[i] == 1)
                    merged[i] = 1;
            }
        }
            
        return merged;
    }
}