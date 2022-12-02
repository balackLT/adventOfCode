using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day14;

public class Solution : ISolution
{
    public int Day { get; } = 14;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();
            
        Debug.Assert(ApplyBitMask(11, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X") == 73);
        Debug.Assert(ApplyBitMask(101, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X") == 101);
        Debug.Assert(ApplyBitMask(0, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X") == 64);

        var maskRegex = new Regex(@"mask = (\w+)");
        var opRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

        var mask = "";
        var memory = new Dictionary<int, long>();

        foreach (var line in lines)
        {
            if (line.StartsWith("mask"))
            {
                mask = maskRegex.Match(line).Groups[1].Value;
            }
            else
            {
                var op = opRegex.Match(line).Groups;
                var number = ApplyBitMask(long.Parse(op[2].Value), mask);
                memory[int.Parse(op[1].Value)] = number;
            }
        }

        var result = memory.Sum(m => m.Value);
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        Debug.Assert(CalculateAddresses(42, "000000000000000000000000000000X1001X").Count == 4);
            
        var maskRegex = new Regex(@"mask = (\w+)");
        var opRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

        var mask = "";
        var memory = new Dictionary<long, long>();

        foreach (var line in lines)
        {
            if (line.StartsWith("mask"))
            {
                mask = maskRegex.Match(line).Groups[1].Value;
            }
            else
            {
                var op = opRegex.Match(line).Groups;
                var number = long.Parse(op[2].Value);

                var addresses = CalculateAddresses(long.Parse(op[1].Value), mask);

                foreach (var address in addresses)
                {
                    memory[address] = number;
                }
            }
        }

        var result = memory.Sum(m => m.Value);
            
        return result.ToString();
    }

    private List<long> CalculateAddresses(long number, string mask)
    {
        var binary = Convert.ToString(number, 2).PadLeft(36, '0').ToList();

        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i] == '1' || mask[i] == 'X')
            {
                binary[i] = mask[i];
            }
        }

        var numbers = new List<string> { string.Concat(binary) };

        while (numbers.Any(n => n.Contains("X")))
        {
            numbers = ExpandFloatingValues(numbers).Distinct().ToList();
        }

        var result = numbers
            .Select(n => Convert.ToInt64(string.Concat(n), 2))
            .ToList();
            
        return result;
    }

    private List<string> ExpandFloatingValues(List<string> list)
    {
        var result = new List<string>();
            
        foreach (var binary in list)
        {
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == 'X')
                {
                    result.Add(ReplaceAt(binary, i, '0'));
                    result.Add(ReplaceAt(binary, i, '1'));
                }
            }       
        }

        return result;
    }
        
    private string ReplaceAt(string input, int index, char newChar)
    {
        var chars = input.ToCharArray();
        chars[index] = newChar;
        return new string(chars);
    }

    private long ApplyBitMask(long number, string mask)
    {
        var binary = Convert.ToString(number, 2).PadLeft(36, '0').ToList();

        for (int i = 0; i < mask.Length; i++)
        {
            if (mask[i] != 'X')
            {
                binary[i] = mask[i];
            }
        }

        var result = Convert.ToInt64(string.Concat(binary), 2);
            
        return result;
    }
}