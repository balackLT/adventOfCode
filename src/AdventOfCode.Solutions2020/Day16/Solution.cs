using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;
using MoreLinq;

namespace AdventOfCode.Solutions2020.Day16
{
    public class Solution : ISolution
    {
        public int Day { get; } = 16;

        public string SolveFirstPart(Input input)
        {
            var lines = input
                .GetLines()
                .Split(s => s == "")
                .ToList();

            var rules = ParseRules(lines);
            
            var nearbyTickets = lines[2].Skip(1).Select(t => t.Split(',').Select(int.Parse));
            
            var result = nearbyTickets.Select(t => DoesNotMatchAnyRule(t, rules)).SelectMany(t => t).Sum();
            
            return result.ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var lines = input
                .GetLines()
                .Split(s => s == "")
                .ToList();

            var rules = ParseRules(lines).ToList();

            var myTicket = lines[1].Skip(1).First().Split(',').Select(int.Parse).ToList();
            var nearbyTickets = lines[2].Skip(1)
                .Select(t => t.Split(',')
                    .Select(int.Parse)
                    .ToList())
                .Where(t => !DoesNotMatchAnyRule(t, rules).Any())
                .ToList();

            var positions = new Dictionary<int, List<Rule>>();
            
            for (var position = 0; position < myTicket.Count; position++)
            {
                var fields = nearbyTickets.Select(t => t[position]).ToList();
                
                var matchedRules = rules
                    .Where(r => r.Matches(fields).Count() == fields.Count)
                    .ToList();

                positions[position] = matchedRules;
            }

            var orderedPositions = positions.OrderBy(p => p.Value.Count).ToList();
            foreach (var position in orderedPositions)
            {
                Debug.Assert(position.Value.Count == 1);

                var rule = position.Value.First();
                
                foreach (var otherPositions in positions.Where(p => p.Value.Count > 1))
                {
                    otherPositions.Value.Remove(rule);
                }
            }
            
            Debug.Assert(positions.All(p => p.Value.Count == 1));

            var resultPositions = positions
                .Where(p => p.Value.First().Name.StartsWith("departure"))
                .Select(p => p.Key)
                .ToList();
            
            Debug.Assert(resultPositions.Count == 6);

            long result = resultPositions.Aggregate<int, long>(1, (current, position) => current * myTicket[position]);

            return result.ToString();
        }

        private static IEnumerable<Rule> ParseRules(List<IEnumerable<string>> lines)
        {
            var ruleRegex = new Regex(@"(.+): (\d+)-(\d+) or (\d+)-(\d+)");

            return lines[0]
                .Select(ruleLine => ruleRegex.Match(ruleLine).Groups.Values.Select(g => g.Value).ToList())
                .Select(match => new Rule
                {
                    Name = match[1],
                    From1 = int.Parse(match[2]),
                    To1 = int.Parse(match[3]),
                    From2 = int.Parse(match[4]),
                    To2 = int.Parse(match[5]),
                });
        }

        private IEnumerable<int> DoesNotMatchAnyRule(IEnumerable<int> ticket, IEnumerable<Rule> rules)
        {
            var unmatchedFields = ticket.ToList();

            foreach (var matches in rules.Select(rule => rule.Matches(unmatchedFields)))
            {
                unmatchedFields.RemoveAll(f => matches.Contains(f));
            }
            
            return unmatchedFields;
        }
        
        private class Rule
        {
            public string Name { get; set; }
            public int From1 { get; set; }
            public int To1 { get; set; }
            public int From2 { get; set; }
            public int To2 { get; set; }
            
            public IEnumerable<int> Matches(IEnumerable<int> ticket)
            {
                return ticket.Where(Matches);
            }
            
            private bool Matches(int ticket)
            {
                return (ticket >= From1 && ticket <= To1) || (ticket >= From2 && ticket <= To2);
            }
        }
    }
}