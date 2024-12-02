using System.Collections.Frozen;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;
using MoreLinq.Extensions;

namespace AdventOfCode.Solutions2023.Day19;

public partial class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines()
            .Split("")
            .ToList();

        var parts = new List<Dictionary<string, int>>();
        foreach (string s in lines[1])
        {
            var part = new Dictionary<string, int>();
            var numbers = IntegersOnly().Matches(s).Select(v => int.Parse(v.Value)).ToList();
            part["x"] = numbers[0];
            part["m"] = numbers[1];
            part["a"] = numbers[2];
            part["s"] = numbers[3];
            parts.Add(part);
        }
        
        Dictionary<string, Workflow> workflows = ParseWorkflows(lines);

        var acceptedParts = parts
            .Where(part => workflows["in"].ProcessPart(part, workflows));

        var output = acceptedParts.Sum(p => p.Sum(kv => kv.Value));
        
        return output;
    }

    private static Dictionary<string, Workflow> ParseWorkflows(List<IEnumerable<string>> lines)
    {
        var workflows = new Dictionary<string, Workflow>();
        foreach (string s in lines[0])
        {
            var indexOfCurly = s.IndexOf('{');
            var name = s[..indexOfCurly];
            
            var stepsString = s[(indexOfCurly + 1)..^1];
            var stepsStringSplit = stepsString.Split(',');

            List<Step> steps = [];
            foreach (string ss in stepsStringSplit)
            {
                if (ss.Contains(':'))
                {
                    // s>1176:kqz
                    var id = ss[0];
                    var operatorChar = ss[1];
                    var rest = ss[2..].Split(':');
                    var value = int.Parse(rest[0]);
                    var result = rest[1];
                
                    var newStep = new Step(id, operatorChar, value, result);
                    steps.Add(newStep);
                }
                else
                {
                    steps.Add(new Step(' ', ' ', 0, ss));
                }
            }
            
            var workflow = new Workflow(name, steps);
            workflows[name] = workflow;
        }

        return workflows;
    }

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex IntegersOnly();

    public record Workflow(string Name, List<Step> Steps)
    {
        public bool ProcessPart(Dictionary<string, int> part, Dictionary<string, Workflow> workflows)
        {
            foreach (Step step in Steps)
            {
                if (step.Id == ' ')
                {
                    return ProcessResult(part, workflows, step);
                }

                var valueToCheck = part[step.Id.ToString()];
                switch (step.Operator)
                {
                    case '>' when valueToCheck > step.Value:
                        return ProcessResult(part, workflows, step);
                    case '<' when valueToCheck < step.Value:
                        return ProcessResult(part, workflows, step);
                }
            }

            return false;
        }

        private static bool ProcessResult(Dictionary<string, int> part, Dictionary<string, Workflow> workflows, Step step)
        {
            if (step.Result == "A")
                return true;
                    
            if (step.Result == "R")
                return false;
                    
            var nextWorkflow = workflows[step.Result];
            return nextWorkflow.ProcessPart(part, workflows);
        }
    }
    
    public record Step(char Id, char Operator, int Value, string Result);
    
    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines()
            .Split("")
            .ToList();
        FrozenDictionary<string, Workflow> workflows = ParseWorkflows(lines).ToFrozenDictionary();

        var ranges = new Dictionary<string, Range>
        {
            ["x"] = new() {Min = 1, Max = 4000},
            ["m"] = new() {Min = 1, Max = 4000},
            ["a"] = new() {Min = 1, Max = 4000},
            ["s"] = new() {Min = 1, Max = 4000}
        };
        
        var result = ProcessPartRanges(workflows, ranges, "in");
        
        return result;
    }
    
    private static long ProcessPartRanges(
        FrozenDictionary<string, Workflow> workflows, 
        Dictionary<string, Range> ranges,
        string workflowName)
    {
        var result = 0L;

        // ranges fully rejected, bye
        if (workflowName == "R")
        {
            return 0;
        }
        
        // ranges fully accepted, yay
        if (workflowName == "A")
        {
            return ranges.Values.Aggregate(1L, (current, value) => current * value.Length);
        }
        
        var workflow = workflows[workflowName];
        foreach (var step in workflow.Steps)
        {
            var currentId = step.Id.ToString();
            // no rules, just go to result with same ranges
            if (currentId == " ")
            {
                result += ProcessPartRanges(workflows, ranges, step.Result);
                return result;
            }
            
            var currentRange = ranges[currentId];
            
            // st{a<466:R,a>1034:A,x<3489:R,R}
            if (step.Operator == '>')
            {
                // all range is valid, process next step without mutating ranges
                if (currentRange.Min > step.Value)
                {
                    result += ProcessPartRanges(workflows, ranges, step.Result);
                    return result;
                }
                
                // no range is valid, return 0, impossible?
                if (currentRange.Max < step.Value)
                {
                    return 0;
                }
                
                // partial range fit, we have to split
                if (currentRange.Min < step.Value)
                {
                    // fitting range can proceed to next rule
                    var fittingRange = new Range {Min = step.Value + 1, Max = currentRange.Max};
                    var newRanges = new Dictionary<string, Range>(ranges)
                    {
                        [currentId] = fittingRange
                    };

                    result += ProcessPartRanges(workflows, newRanges, step.Result);
                    
                    // unfitting part of range goes to next step -> mutate current range
                    var unfittingRange = new Range {Min = currentRange.Min, Max = step.Value};
                    ranges[currentId] = unfittingRange;
                }
            }
            else if (step.Operator == '<')
            {
                if (currentRange.Max < step.Value)
                {
                    result += ProcessPartRanges(workflows, ranges, step.Result);
                    return result;
                }
                
                if (currentRange.Min > step.Value)
                {
                    return 0;
                }
                
                if (currentRange.Max > step.Value)
                {
                    var fittingRange = new Range {Min = currentRange.Min, Max = step.Value - 1};
                    var newRanges = new Dictionary<string, Range>(ranges)
                    {
                        [currentId] = fittingRange
                    };

                    result += ProcessPartRanges(workflows, newRanges, step.Result);
                    
                    var unfittingRange = new Range {Min = step.Value, Max = currentRange.Max};
                    ranges[currentId] = unfittingRange;
                }
            }
            
        }
        
        return result;
    }
    
    private class Range
    {
        public int Min { get; init; }
        public int Max { get; init; }
        
        public int Length => Max - Min + 1;
    }
}