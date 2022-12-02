using System.Diagnostics;
using System.Text;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day10;

public class Solution : ISolution
{
    public int Day { get; } = 10;

    public string SolveFirstPart(Input input)
    {
        var number = input.GetAsString();

        Debug.Assert(SayNextNumber("1") == "11");
        Debug.Assert(SayNextNumber("1211") == "111221");
        Debug.Assert(SayNextNumber("111221") == "312211");
        Debug.Assert(SayNextNumber("1111111111") == "101");
            
        for (int i = 0; i < 40; i++)
        {
            number = SayNextNumber(number);
        }
            
        return number.Length.ToString();
    }

    private string SayNextNumber(string number)
    {
        var sb = new StringBuilder();
            
        var previous = ' ';
        var count = 1;
        foreach (var element in number)
        {
            if (element == previous)
            {
                count++;
            }
            else if (previous != ' ')
            {
                sb.Append($"{count}{previous}");
                count = 1;
            }

            previous = element;
        }
        sb.Append($"{count}{previous}");

        return sb.ToString();
    }
        
        
    public string SolveSecondPart(Input input)
    {
        var number = input.GetAsString();

        for (int i = 0; i < 50; i++)
        {
            number = SayNextNumber(number);
        }
            
        return number.Length.ToString();
    }
}