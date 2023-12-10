using System;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day02;

public class Solution :ISolution
{
    public int Day { get; } = 2;
        
    public object SolveFirstPart(Input input)
    {
        Debug.Assert(CalculateAreaWithSlack(2, 3, 4) == 58);
            
        var boxes = input
            .GetLines()
            .Select(b => b.Split('x').Select(int.Parse).ToArray())
            .Select(b => (L: b[0], W: b[1], H: b[2]));

        var result = boxes.Sum(b => CalculateAreaWithSlack(b.L, b.W, b.H));
            
        return result.ToString();
    }

    private int CalculateAreaWithSlack(int l, int w, int h)
    {
        var side1 = l * w;
        var side2 = w * h;
        var side3 = h * l; 
            
        var area =  2 * side1 + 2 * side2 + 2 * side3 + Math.Min(side1, Math.Min(side2, side3));

        return area;
    }
        
    public object SolveSecondPart(Input input)
    {
        var boxes = input
            .GetLines()
            .Select(b => b.Split('x').Select(int.Parse).ToArray())
            .Select(b => (L: b[0], W: b[1], H: b[2]));
            
        var result = boxes.Sum(b => CalculateRibbon(b.L, b.W, b.H));
            
        return result.ToString();
    }

    private int CalculateRibbon(int l, int w, int h)
    {
        var perimeter1 = 2 * l + 2 * w;
        var perimeter2 = 2 * w + 2 * h;
        var perimeter3 = 2 * h + 2 * l;

        var ribbon = Math.Min(perimeter1, Math.Min(perimeter2, perimeter3));
            
        var bow = l * w * h;

        return ribbon + bow;
    }
}