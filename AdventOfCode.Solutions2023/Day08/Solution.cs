using AdventOfCode.Executor;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions2023.Day08;

public class Solution : ISolution
{
    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var instructions = lines.First();
        
        var nodes = new Dictionary<string, Node>();
        foreach (var line in lines.Skip(2))
        {
            var current = GetOrCreate(nodes, line[..3]);
            var left = GetOrCreate(nodes, line[7..10]);
            var right = GetOrCreate(nodes, line[12..15]);
            
            current.Left = left;
            current.Right = right;
        }

        var currentNode = nodes["AAA"];
        var steps = 0L;
        while (true)
        {
            foreach (var instruction in instructions)
            {
                steps++;
                currentNode = instruction == 'L' ? currentNode.Left! : currentNode.Right!;
                if (currentNode.Name == "ZZZ")
                {
                    return steps.ToString();
                }
            }
        }
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var instructions = lines.First();
        
        var nodes = new Dictionary<string, Node>();
        foreach (var line in lines.Skip(2))
        {
            var current = GetOrCreate(nodes, line[..3]);
            var left = GetOrCreate(nodes, line[7..10]);
            var right = GetOrCreate(nodes, line[12..15]);
            
            current.Left = left;
            current.Right = right;
        }

        var currentNodes = nodes
            .Where(n => n.Key.EndsWith('A'))
            .Select(n => n.Value)
            .ToList();
        
        var steps = 0L;

        var distances = new List<long>();
        while (true)
        {
            foreach (var instruction in instructions)
            {
                steps++;
                var newCurrentNodes = new List<Node>();
                for(var i = 0; i < currentNodes.Count; i++)
                {
                    var newNode = instruction == 'L' ? currentNodes[i].Left! : currentNodes[i].Right!;
                    if (newNode.Name.EndsWith('Z'))
                    {
                        Console.WriteLine($"Node {i} reached Z in {steps} steps");
                        distances.Add(steps);
                    }
                    else
                    {
                        newCurrentNodes.Add(newNode);
                    }
                }
                
                // this probably doesn't work for the general case, but this is usually the answer in AoC
                // I tried it and it worked :D
                // the assumption is the cycles continue in the same way, but I'm not gonna try to prove it
                if (newCurrentNodes.Count == 0)
                {
                    return AocMath.Lcm(distances).ToString();
                }
                currentNodes = newCurrentNodes;
            }
        }
    }
    
    private static Node GetOrCreate(Dictionary<string, Node> nodes, string name)
    {
        if (nodes.TryGetValue(name, out Node? newNode))
        {
            return newNode;
        }

        var node = new Node
        {
            Name = name,
            Left = null,
            Right = null
        };
        nodes.Add(name, node);
        return node;
    }

    public class Node
    {
        public string Name { get; init; }
        public Node? Left { get; set; }
        public Node? Right { get; set; }
    }
}