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

        var acceptedParts = parts
            .Where(part => workflows["in"].ProcessPart(part, workflows));

        var output = acceptedParts.Sum(p => p.Sum(kv => kv.Value));
        
        return output;
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
        var lines = input.GetLines();

        
        return 0;
    }
}