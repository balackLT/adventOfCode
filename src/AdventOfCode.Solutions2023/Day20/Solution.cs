using System.Collections.Frozen;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day20;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines().ToList();

        var modules = ParseModules(lines).ToFrozenDictionary();

        List<SignalDestination> signalsSent = [];
        for (int i = 0; i < 1000; i++)
        {
            var firstSignal = new SignalDestination(Signal.LOW, "roadcaster", "button");
            var destinations = new Queue<SignalDestination>();
            destinations.Enqueue(firstSignal);

            while (destinations.Count > 0)
            {
                var destination = destinations.Dequeue();
                signalsSent.Add(destination);
                
                var target = modules[destination.Destination];
                
                var newDestinations = target.ProcessSignal(destination.Signal, destination.source, modules);
                while (newDestinations.Count > 0)
                {
                    destinations.Enqueue(newDestinations.Dequeue());
                }
            }
        }

        var result = signalsSent.Count(s => s.Signal == Signal.HIGH) * 
                     signalsSent.Count(s => s.Signal == Signal.LOW);

        return result;
    }

    private static Dictionary<string, Module> ParseModules(List<string> lines)
    {
        var modules = new Dictionary<string, Module>();
        
        foreach (string line in lines)
        {
            //&fb -> hb, vk, fz, kl, cg
            var split = line.Split(" -> ");
            var type = split[0][0];
            var name = split[0][1..];
            var destinations = split[1].Split(", ");
            
            var module = new Module
            {
                Name = name,
                Type = type,
                Destinations = destinations.ToList()
            };
            
            modules.Add(name, module);
        }

        foreach (var module in modules.Where(m => m.Value.Type == '&'))
        {
            var sources = modules
                .Where(m => m.Value.Destinations.Contains(module.Key));

            foreach (var source in sources)
            {
                module.Value.Memory[source.Key] = Signal.LOW;
            }
        }
        
        var allDestinations = modules.SelectMany(m => m.Value.Destinations).Distinct().ToList();
        var allKnownModules = modules.Keys.ToList();
        var unknownModules = allDestinations.Except(allKnownModules).ToList();
        foreach (string unknownModule in unknownModules)
        {
            modules.Add(unknownModule, new Module {Name = unknownModule, Type = 'b'});
        }

        return modules;
    }

    private class Module 
    {
        public string Name { get; init; } = "";
        public char Type { get; init; }
        public bool On { get; set; } = false;
        public Dictionary<string, Signal> Memory { get; init; } = [];
        public List<string> Destinations { get; init; } = [];
        

        public Queue<SignalDestination> ProcessSignal(Signal signal, string from, FrozenDictionary<string, Module> modules)
        {
            var result = new Queue<SignalDestination>();
            
            if (Type == 'b')
            {
                foreach (Module? module in Destinations.Select(destination => modules[destination]))
                {
                    result.Enqueue(new SignalDestination(signal, module.Name, Name));
                }
            }
            else if (Type == '%')
            {
                if (signal == Signal.LOW)
                {
                    if (On)
                    {
                        foreach (Module? module in Destinations.Select(destination => modules[destination]))
                        {
                            result.Enqueue(new SignalDestination(Signal.LOW, module.Name, Name));
                        }
                    }
                    else
                    {
                        foreach (Module? module in Destinations.Select(destination => modules[destination]))
                        {
                            result.Enqueue(new SignalDestination(Signal.HIGH, module.Name, Name));
                        }
                    }
                    On = !On;
                }
            }
            else if (Type == '&')
            {
                Memory[from] = signal;
                if (Memory.All(m => m.Value == Signal.HIGH))
                {
                    foreach (Module? module in Destinations.Select(destination => modules[destination]))
                    {
                        result.Enqueue(new SignalDestination(Signal.LOW, module.Name, Name));
                    }
                }
                else
                {
                    foreach (Module? module in Destinations.Select(destination => modules[destination]))
                    {
                        result.Enqueue(new SignalDestination(Signal.HIGH, module.Name, Name));
                    }
                }
            }
            
            return result;
        }
    }
    
    private record SignalDestination(Signal Signal, string Destination, string source);

    private enum Signal
    {
        HIGH,
        LOW
    }
    
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines().ToList();

        var modules = ParseModules(lines).ToFrozenDictionary();

        HashSet<string> interestingModules = ["xf", "fz", "mp", "hn"];
        
        var signalsSent = new Dictionary<SignalDestination, long>();
        for (int i = 0; i < 1000; i++)
        {
            var firstSignal = new SignalDestination(Signal.LOW, "roadcaster", "button");
            var destinations = new Queue<SignalDestination>();
            destinations.Enqueue(firstSignal);

            while (destinations.Count > 0)
            {
                var destination = destinations.Dequeue();
                
                if (destination is { Destination: "rx", Signal: Signal.LOW })
                {
                    return i;
                }
                
                if (interestingModules.Contains(destination.source) && destination.Signal == Signal.HIGH)
                {
                    Console.WriteLine($"{destination.source} sent {destination.Signal} to {destination.Destination} at {i}");
                }

                if (signalsSent.TryGetValue(destination, out var count))
                {
                    signalsSent[destination] = count + 1;
                }
                else
                {
                    signalsSent.Add(destination, 1);
                }
                
                var target = modules[destination.Destination];
                
                var newDestinations = target.ProcessSignal(destination.Signal, destination.source, modules);
                while (newDestinations.Count > 0)
                {
                    var newDestination = newDestinations.Dequeue();
                    destinations.Enqueue(newDestination);
                    
                    if (target.Type == '&' && newDestination.Signal == Signal.HIGH)
                    {
                        Console.WriteLine($"{destination.Destination} received {newDestination.Signal} from {newDestination.source} at {i}");
                    }
                }
            }
        }

        return 0;
    }
}