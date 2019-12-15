using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day07
{
    public class Wire
    {
        public Wire SourceA;
        public Wire SourceB;
        public string Operation;
        public string Name;
        public int? Value = null;

        public static Wire ConstructChain(string start, Dictionary<string, string> inputs)
        {
            // Console.WriteLine($"Constructing {start}");
            
            if (int.TryParse(start, out var valueWire))
                return new Wire {Name = "FAKE", Value = valueWire};
            
            var wire = new Wire {Name = start};

            var source = inputs[start];

            var split = source.Split(' ');
            
            switch (split.Count())
            {
                case 1:
                    if (int.TryParse(source, out var value))
                        wire.Value = value;
                    else wire.SourceA = ConstructChain(split[0], inputs);
                    return wire;
                case 2:
                    wire.Operation = split[0];
                    wire.SourceA = ConstructChain(split[1], inputs);
                    return wire;
                case 3:
                    wire.Operation = split[1];
                    wire.SourceA = ConstructChain(split[0], inputs);
                    wire.SourceB = ConstructChain(split[2], inputs);
                    return wire;
                default:
                    throw new Exception("Unexpected source format");
            }
        }
    }
    
    public class Solution : ISolution
    {
        public int Day { get; } = 7;
        
        public string SolveFirstPart(Input input)
        {
            var inputs = input.GetLines()
                .Select(l => l.Split(" -> "))
                .Select(l => new KeyValuePair<string, string>(l[1], l[0]))
                .ToDictionary(key => key.Key, value => value.Value);
            
            var wireA = Wire.ConstructChain("a", inputs);
            
            return 0.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            return 0.ToString();
        }
    }
}