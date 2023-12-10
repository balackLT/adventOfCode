using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day11;

public class Solution : ISolution
{
    public int Day { get; } = 11;

    public object SolveFirstPart(Input input)
    {
        var password = input.GetAsString();
            
        Debug.Assert(GetNextPassword("aaa") == "aab");
        Debug.Assert(GetNextPassword("aaz") == "aba");
        Debug.Assert(GetNextPassword("azz") == "baa");
            
        Debug.Assert(IsValidPassword("abcdffaa") == true);
        Debug.Assert(IsValidPassword("hijklmmn") == false);
        Debug.Assert(IsValidPassword("abbceffg") == false);
        Debug.Assert(IsValidPassword("abbcegjk") == false);

        var result = password;
            
        while (true)
        {
            result = GetNextPassword(result);
            if (IsValidPassword(result))
                break;
        }
            
        return result;
    }
        
    public object SolveSecondPart(Input input)
    {
        var password = input.GetAsString();

        var result = password;
            
        while (true)
        {
            result = GetNextPassword(result);
            if (IsValidPassword(result))
                break;
        }
            
        while (true)
        {
            result = GetNextPassword(result);
            if (IsValidPassword(result))
                break;
        }
            
        return result;
    }

    private bool IsValidPassword(string password)
    {
        var hasSequence = false;
        var doubles = 0;

        var prev1 = ' ';
        var prev2 = ' ';
        var lastDouble = false;
            
        foreach (char letter in password)
        {
            if (letter == 'i' || letter == 'l' || letter == 'o')
                return false;

            if (letter == prev1 + 1 && letter == prev2 + 2)
                hasSequence = true;

            if (!lastDouble && letter == prev1)
            {
                doubles++;
                lastDouble = true;
            }
            else
            {
                lastDouble = false;
            }

            prev2 = prev1;
            prev1 = letter;
        }

        return hasSequence && doubles >= 2;
    }
        
    private string GetNextPassword(string password)
    {
        var newPassword = new List<char>();

        var overflow = true;
        foreach (var letter in password.Reverse())
        {
            if (overflow)
            {
                char newLetter;
                if (letter == 'z')
                {
                    newLetter = 'a';
                    overflow = true;
                }
                else
                {
                    newLetter = (char)(letter + 1);
                    overflow = false;
                }
                newPassword.Add(newLetter);
            }
            else
            {
                newPassword.Add(letter);
            }
        }

        newPassword.Reverse();
            
        return new string(newPassword.ToArray());
    }
}