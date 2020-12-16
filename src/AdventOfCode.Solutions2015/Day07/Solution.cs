using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day07
{
    public class Solution : ISolution
    {
        public int Day { get; } = 7;
        
        public string SolveFirstPart(Input input)
        {
            var inputs = input.GetLines()
                .Select(l => l.Split(" -> "))
                .Select(l => new KeyValuePair<string, string>(l[1], l[0]))
                .ToDictionary(key => key.Key, value => value.Value);

            var wires = new Dictionary<string, Wire>();
            
            var wireA = ConstructChain("a", inputs, wires);

            var result = wireA.Calculate();
            
            return result.ToString();
        }

        public string SolveSecondPart(Input input)
        {
            var inputs = input.GetLines()
                .Select(l => l.Split(" -> "))
                .Select(l => new KeyValuePair<string, string>(l[1], l[0]))
                .ToDictionary(key => key.Key, value => value.Value);

            var wires = new Dictionary<string, Wire>();
            
            var wireA = ConstructChain("a", inputs, wires);
            Debug.Assert(wires["b"].Operation == "CONSTANT");
            
            wires["b"].Value = wireA.Calculate();
            foreach (var wire in wires)
            {
                wire.Value.CachedValue = null;
            }
            
            var result = wireA.Calculate();
            
            return result.ToString();
        }
        
        private class Wire
        {
            public Wire SourceA { get; set; }
            public Wire SourceB { get; set; }
            public string Operation { get; set; }
            public string Name { get; set; }
            public int Value { get; set; }
            
            public int? CachedValue { get; set; }

            public int Calculate()
            {
                if (CachedValue is not null)
                    return (int)CachedValue;
                
                var result = Operation switch
                {
                    "CONSTANT" => Value,
                    "NOT" => ~SourceA.Calculate(),
                    "AND" => SourceA.Calculate() & SourceB.Calculate(),
                    "OR" => SourceA.Calculate() | SourceB.Calculate(),
                    "LSHIFT" => SourceA.Calculate() << SourceB.Calculate(),
                    "RSHIFT" => SourceA.Calculate() >> SourceB.Calculate(),
                    _ => SourceA.Calculate()
                };

                CachedValue = result;

                return result;
            }
        }
        
        private static Wire ConstructChain(string start, IDictionary<string, string> inputs, IDictionary<string, Wire> wires)
        {
            if (int.TryParse(start, out var valueWire))
                return new Wire {Name = "CONSTANT", Operation = "CONSTANT", Value = valueWire};
            
            if (wires.TryGetValue(start, out var result))
                return result;
            
            var wire = new Wire
            {
                Name = start
            };

            wires.Add(start, wire);
            
            var source = inputs[start];

            var split = source.Split(' ');
            
            switch (split.Length)
            {
                case 1:
                    if (int.TryParse(source, out var value))
                    {
                        wire.Operation = "CONSTANT";
                        wire.Value = value;
                    }
                    else wire.SourceA = ConstructChain(split[0], inputs, wires);
                    return wire;
                case 2:
                    wire.Operation = split[0];
                    wire.SourceA = ConstructChain(split[1], inputs, wires);
                    return wire;
                case 3:
                    wire.Operation = split[1];
                    wire.SourceA = ConstructChain(split[0], inputs, wires);
                    wire.SourceB = ConstructChain(split[2], inputs, wires);
                    return wire;
                default:
                    throw new Exception("Unexpected source format");
            }
        }
    }
}