using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day13;

public class Solution : ISolution
{
    public int Day { get; } = 13;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        HashSet<Person> people = ParsePeople(lines);

        var result = people.First().FindBestPath(new List<string>(), 0);
            
        return result.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        HashSet<Person> people = ParsePeople(lines);
        var me = new Person("Santa");
        me.Affection = people
            .Select(p => new KeyValuePair<Person, int>(p, 0))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var person in people)
        {
            person.Affection.Add(me, 0);
        }

        var result = me.FindBestPath(new List<string>(), 0);
            
        return result.ToString();
    }

    private HashSet<Person> ParsePeople(string[] lines)
    {
        var people = new HashSet<Person>();

        foreach (var line in lines)
        {
            var words = line.Split();
            var name = words[0];
            var target = words[^1].TrimEnd('.');
            var sign = words[2] == "gain" ? '+' : '-';
            var amount = int.Parse(sign + words[3]);

            var person = people.Any(p => p.Name == name) ? people.First(p => p.Name == name) : new Person(name);
            var targetPerson = people.Any(p => p.Name == target) ? people.First(p => p.Name == target) : new Person(target);

            people.Add(person);
            people.Add(targetPerson);

            person.Affection[targetPerson] = amount;
        }

        return people;
    }
        
    private record Person(string Name)
    {
        public Dictionary<Person, int> Affection = new();

        public int FindBestPath(List<string> path, int distanceTraveled)
        {
            path.Add(Name);
                
            var notVisited = Affection
                .Where(p => !path.Contains(p.Key.Name))
                .ToList();

            if (notVisited.Count == 0)
            {
                var pathStart = Affection.FirstOrDefault(n => n.Key.Name == path.First());
                return distanceTraveled + pathStart.Value + pathStart.Key.Affection[this];
            }

            var bestPath = int.MinValue;
                
            foreach (var neighbour in notVisited)
            {
                var distance = neighbour.Value + neighbour.Key.Affection[this];
                bestPath = Math.Max(neighbour.Key.FindBestPath(new List<string>(path), distance), bestPath);
            }
                
            return distanceTraveled + bestPath;
        }
    }
}