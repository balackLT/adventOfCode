using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day02;

public class Solution : ISolution
{
    public int Day { get; } = 2;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLinesByRegex(@"(\d+)-(\d+) (\w): (.+)");
        var passwords = new List<Password>();
            
        foreach (var line in lines)
        {
            var password = new Password
            {
                From = int.Parse(line[1]),
                To = int.Parse(line[2]),
                Letter = line[3].FirstOrDefault(),
                Code = line[4]
            };

            passwords.Add(password);
        }

        var result = passwords.Count(p => p.IsValid());
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLinesByRegex(@"(\d+)-(\d+) (\w): (.+)");
        var passwords = new List<Password>();
            
        foreach (var line in lines)
        {
            var password = new Password
            {
                From = int.Parse(line[1]),
                To = int.Parse(line[2]),
                Letter = line[3].FirstOrDefault(),
                Code = line[4]
            };

            passwords.Add(password);
        }

        var result = passwords.Count(p => p.IsValid2());
            
        return result.ToString();
    }

    private class Password
    {
        public int From { get; set; }
        public int To { get; set; }
        public char Letter { get; set; }
        public string Code { get; set; }

        public bool IsValid()
        {
            var count = Code.Count(l => l == Letter);

            return count >= From && count <= To;
        }
            
        public bool IsValid2()
        {
            return Code[From - 1] == Letter ^ Code[To - 1] == Letter;
        }
    }
}