using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using MoreLinq;

namespace AdventOfCode.Solutions2020.Day19
{
    public class Solution : ISolution
    {
        public int Day { get; } = 19;

        public string SolveFirstPart(Input input)
        {
            var lines = input
                .GetLines()
                .Split(l => l == string.Empty)
                .ToList();

            Dictionary<int, Rule> rules = ParseRules(lines);
            
            var result = 0;
            foreach (var message in lines[1])
            {
                var allowed = rules[0].GenerateAllowed(rules, message).ToList();
                if (allowed.Contains(message))
                    result++;
            }
            
            return result.ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var lines = input
                .GetLines()
                .Split(l => l == string.Empty)
                .ToList();

            Dictionary<int, Rule> rules = ParseRules(lines);
            
            rules[8] = new Rule(8, null, "42 | 42 8");
            rules[11] = new Rule(11, null, "42 31 | 42 11 31");
            
            var result = 0;
            foreach (var message in lines[1])
            {
                var allowed = rules[0].GenerateAllowed(rules, message).ToList();
                if (allowed.Contains(message))
                    result++;
            }
            
            return result.ToString();
        }
        
        private static Dictionary<int, Rule> ParseRules(List<IEnumerable<string>> lines)
        {
            var rules = new Dictionary<int, Rule>();

            foreach (var rule in lines[0])
            {
                var split = rule.Split(':');
                char? value = null;
                string restriction = null;

                if (split[1].Contains('"'))
                    value = split[1][2];
                else restriction = split[1];

                rules.Add(int.Parse(split[0]), new Rule(int.Parse(split[0]), value, restriction));
            }

            return rules;
        }
       
        
        private record Rule(int Number, char? Value, string Restriction)
        {
            public IEnumerable<string> GenerateAllowed(IDictionary<int, Rule> rules, string message)
            {
                if (Value is not null)
                    yield return Value.ToString();
                else
                {
                    var branches = Restriction.Split('|', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var branch in branches.Select(b => b.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                    {
                        switch (branch.Length)
                        {
                            case 1:
                            {
                                var rule = rules[int.Parse(branch[0])];
                                foreach (var variation in rule.GenerateAllowed(rules, message))
                                {
                                    yield return variation;
                                }

                                break;
                            }
                            case 2:
                            {
                                var rule1 = rules[int.Parse(branch[0])];
                                var rule2 = rules[int.Parse(branch[1])];
                                var allowed1 = rule1.GenerateAllowed(rules, message);

                                foreach (var first in allowed1)
                                {
                                    if (!message.StartsWith(first))
                                        continue;
                                
                                    var allowed2 = rule2.GenerateAllowed(rules, message.Substring(first.Length));

                                    foreach (var second in allowed2)
                                    {
                                        yield return first + second;
                                    }
                                }

                                break;
                            }
                            default:
                            {
                                var rule1 = rules[int.Parse(branch[0])];
                                var rule2 = rules[int.Parse(branch[1])];
                                var rule3 = rules[int.Parse(branch[2])];
                                var allowed1 = rule1.GenerateAllowed(rules, message);
                                
                                foreach (var first in allowed1)
                                {
                                    if (!message.StartsWith(first))
                                        continue;
                                
                                    var allowed2 = rule2.GenerateAllowed(rules, message.Substring(first.Length));

                                    foreach (var second in allowed2)
                                    {
                                        if (!message.StartsWith(first + second))
                                            continue;
                                    
                                        var allowed3 = rule3.GenerateAllowed(rules, message.Substring(first.Length + second.Length));

                                        foreach (var third in allowed3)
                                        {
                                            yield return first + second + third;
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}