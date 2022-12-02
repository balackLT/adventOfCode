using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day13;

public class Solution : ISolution
{
    public int Day { get; } = 13;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var target = int.Parse(lines[0]);
        var busses = lines[1]
            .Split(',')
            .Where(b => b != "x")
            .Select(int.Parse);

        var closest = int.MaxValue;
        var closestBus = 0;

        foreach (var bus in busses)
        {
            var distance = bus * (target / bus) + bus; 
                
            // assumption: no bus is directly on time
            if (distance < closest)
            {
                closest = distance;
                closestBus = bus;
            }
        }

        var result = (closest - target) * closestBus;
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var timetable = input
            .GetLines()[1]
            .Split(',');

        var equation = string.Empty;
        var letters = new Queue<char>("ABCDEFGHJKLMNOPQRSTUVWXYZ");

        var constraints = new List<Constraint>();
            
        for (var i = 0; i < timetable.Length; i++)
        {
            if (timetable[i] == "x")
                continue;
                
            equation += $" {int.Parse(timetable[i])}{letters.Dequeue()} = t + {i};";
            constraints.Add(new Constraint
            {
                Divisor = long.Parse(timetable[i]),
                Offset = i
            });
        }

        // The returned equation system can be solved by Wolfram Alpha
        // Check the form of "t" in "integer solutions"
        Console.WriteLine(equation);
        Console.WriteLine();

        var result = FindIntegerSolution(constraints);
            
        return result.ToString();
    }

    private long FindIntegerSolution(List<Constraint> constraints)
    {
        long t = 0;

        var modulos = new Queue<Constraint>(constraints);
        var currentModulo = modulos.Dequeue();
        var increment = currentModulo.Divisor;
        currentModulo = modulos.Dequeue();

        while (true)
        {
            if (constraints.All(constraint => (t + constraint.Offset) % constraint.Divisor <= 0)) 
                return t;

            if ((t + currentModulo.Offset) % currentModulo.Divisor == 0)
            {
                increment *= currentModulo.Divisor;
                Console.WriteLine($"Modulo {currentModulo.Divisor} found at {t}. Current increment: {increment}");
                currentModulo = modulos.Dequeue();
            }
                
            t += increment;
        }
    }

    private record Constraint
    {
        public long Divisor;
        public long Offset;
    }
}