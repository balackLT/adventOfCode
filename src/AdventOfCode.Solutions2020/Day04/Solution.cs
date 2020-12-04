using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;
using NUnit.Framework;

namespace AdventOfCode.Solutions2020.Day04
{
    public class Solution : ISolution
    {
        public int Day { get; } = 4;

        public string SolveFirstPart(Input input)
        {
            var lines = input.GetLinesAsList();

            var passports = ExtractPassports(lines);

            var result = passports.Count(IsValid);
            
            return result.ToString();
        }

        
        public string SolveSecondPart(Input input)
        {
            var lines = input.GetLinesAsList();

            var passports = ExtractPassports(lines);
            
            var result = passports.Count(IsValid2);
            
            return result.ToString();
        }

        private static List<Dictionary<string, string>> ExtractPassports(List<string> lines)
        {
            var passports = new List<Dictionary<string, string>>();
            var passport = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    var attributes = line
                        .Split(' ')
                        .Select(a => a.Split(':'))
                        .Select(a => new KeyValuePair<string, string>(a[0], a[1]));

                    foreach (var attribute in attributes)
                    {
                        passport.Add(attribute.Key, attribute.Value);
                    }
                }
                else
                {
                    passports.Add(passport);
                    passport = new Dictionary<string, string>();
                }
            }

            passports.Add(passport);
            return passports;
        }
        
        private bool IsValid2(Dictionary<string, string> passport)
        {
            if (passport.TryGetValue("byr", out var byr))
            {
                if (byr.Length != 4) return false;
                if (int.Parse(byr) < 1920 || int.Parse(byr) > 2002)
                    return false;
            }
            else return false;

            if (passport.TryGetValue("iyr", out var iyr))
            {
                if (iyr.Length != 4) return false;
                if (int.Parse(iyr) < 2010 || int.Parse(iyr) > 2020)
                    return false;
            }
            else return false;

            if (passport.TryGetValue("eyr", out var eyr))
            {
                if (eyr.Length != 4) return false;
                if (int.Parse(eyr) < 2020 || int.Parse(eyr) > 2030)
                    return false;
            }
            else return false;
            
            if (passport.TryGetValue("hgt", out var hgt))
            {
                if (hgt.EndsWith("cm"))
                {
                    hgt = hgt.Remove(hgt.Length - 2);
                    if (int.Parse(hgt) < 150 || int.Parse(hgt) > 193)
                        return false;
                }
                else if (hgt.EndsWith("in"))
                {
                    hgt = hgt.Remove(hgt.Length - 2);
                    if (int.Parse(hgt) < 59 || int.Parse(hgt) > 76)
                        return false;
                }
                else return false;
            }
            else return false;
            
            if (passport.TryGetValue("hcl", out var hcl))
            {
                var r = new Regex("^#[a-z0-9]*$");
                if (hcl.Length != 7) return false;
                if (!r.IsMatch(hcl)) return false;
            }
            else return false;
            
            if (passport.TryGetValue("ecl", out var ecl))
            {
                var validColors = new List<string> {"amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                if (!validColors.Contains(ecl)) return false;
            }
            else return false;
            
            if (passport.TryGetValue("pid", out var pid))
            {
                var r = new Regex("^[0-9]*$");
                if (pid.Length != 9) return false;
                if (!r.IsMatch(pid)) return false;
            }
            else return false;
            
            return true;
        }
        
        private bool IsValid(Dictionary<string, string> passport)
        {
            var validKeys = new List<string>
            {
                "byr",
                "iyr",
                "eyr",
                "hgt", 
                "hcl",
                "ecl",
                "pid",
                //"cid" 
            };

            return validKeys.All(passport.ContainsKey);
        }
    }
}