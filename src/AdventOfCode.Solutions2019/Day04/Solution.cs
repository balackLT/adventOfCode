using System.Linq;
using AdventOfCode.Executor;
using NUnit.Framework;

namespace AdventOfCode.Solutions2019.Day04;

public class Solution : ISolution
{
    public int Day { get; } = 4;
        
    public object SolveFirstPart(Input input)
    {
        var range = input.GetAsString().Split('-').Select(int.Parse).ToArray();
        var left = range[0];
        var right = range[1];

        var count = 0;
            
        for (int password = left; password <= right; password++)
        {
            if (IsValidPassword(password))
                count++;
        }
            
        return count.ToString();
    }

    private bool IsValidPassword(int password)
    {
        var intList = password.ToString().Select(digit => int.Parse(digit.ToString()));

        var previousDigit = 0;
        var sameAdjacent = false;
            
        foreach (var i in intList)
        {
            if (previousDigit > i)
                return false;
                
            if (!sameAdjacent && (previousDigit == i))
                sameAdjacent = true;
                
            previousDigit = i;
        }
            
        return sameAdjacent;
    }

    public object SolveSecondPart(Input input)
    {
        var range = input.GetAsString().Split('-').Select(int.Parse).ToArray();
        var left = range[0];
        var right = range[1];

        var count = 0;
            
        Assert.AreEqual(true , IsValidPassword2(112233));
        Assert.AreEqual(false, IsValidPassword2(123444));
        Assert.AreEqual(true , IsValidPassword2(111122));
        Assert.AreEqual(true , IsValidPassword2(112222));
        Assert.AreEqual(true , IsValidPassword2(122233));
        Assert.AreEqual(true , IsValidPassword2(123466));
        Assert.AreEqual(false, IsValidPassword2(133345));
        Assert.AreEqual(false, IsValidPassword2(555555));
            
        for (int password = left; password <= right; password++)
        {
            if (IsValidPassword2(password))
                count++;
        }
            
        return count.ToString();
    }
        
    private bool IsValidPassword2(int password)
    {
        var intList = password.ToString().Select(digit => int.Parse(digit.ToString())).ToArray();

        var previousDigit = intList[0];

        for (var i = 1; i < 6; i++)
        {
            if (previousDigit > intList[i])
                return false;

            previousDigit = intList[i];
        }
            
        previousDigit = intList[0];
        var prevprevdigit = 0;
            
        for (var i = 1; i < 5; i++)
        {
            if ((intList[i] == previousDigit) && (intList[i] != intList[i + 1]) && (intList[i] != prevprevdigit))
                return true;

            prevprevdigit = previousDigit;
            previousDigit = intList[i];
        }

        if (intList[5] == intList[4] && intList[4] != intList[3])
            return true;

        return false;
    }
}