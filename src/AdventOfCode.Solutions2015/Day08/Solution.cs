using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day08;

public class Solution : ISolution
{
    public int Day { get; } = 8;
        
    public object SolveFirstPart(Input input)
    {
        var inputs = input.GetLines();

        Debug.Assert(Escape(@"\x27bbb") == "'bbb");
        Debug.Assert(Escape(@"\\\\") == @"\\");
        Debug.Assert(Escape(@"\\\\b") == @"\\b");
        Debug.Assert(Escape(@"\\\") == @"\\");
        Debug.Assert(Escape(@"\\") == @"\");
        Debug.Assert(Escape(@"\""") == @"""");
            
        var memoryLength = inputs.Sum(l => l.Length);
        var actualLength = inputs.Sum(l => Escape(l).Length - 2);

        var result = memoryLength - actualLength;
            
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var inputs = input.GetLines();
            
        var memoryLength = inputs.Sum(l => l.Length);
        var encodedLength = inputs.Sum(l => Encode(l).Length + 2);

        var result = encodedLength - memoryLength;
            
        return result.ToString();
    }

    private static string Encode(string line)
    {
        var sb = new StringBuilder();

        foreach (var character in line)
        {
            switch (character)
            {
                case '"':
                    sb.Append(@"\""");
                    break;
                case '\\':
                    sb.Append(@"\\");
                    break;
                default:
                    sb.Append(character);
                    break;
            }
        }

        return sb.ToString();
    }

    private static string Escape(string line)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < line.Length; i++)
        {
            if (i + 2 > line.Length)
            {
                sb.Append(line[i]);
            }
            else switch (line[i..(i + 2)])
            {
                case @"\\":
                    sb.Append('\\');
                    i++;
                    break;
                case @"\""":
                    sb.Append('"');
                    i++;
                    break;
                case @"\x":
                {
                    var hex = "0" + line[(i + 1)..(i + 4)];

                    var intValue = Convert.ToInt32(hex, 16);
                    var character = Convert.ToChar(intValue);
                    
                    sb.Append(character);
                    i += 3;
                    break;
                }
                default:
                    sb.Append(line[i]);
                    break;
            }
        }

        return sb.ToString();
    }
}