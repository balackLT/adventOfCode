using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2015.Day04;

public class Solution : ISolution
{
    public int Day { get; } = 4;
        
    public string SolveFirstPart(Input input)
    {
        var key = input.GetAsString();

        var result = 0;
        while (true)
        {
            result++;
            var tempKey = key + result;

            var hash = CalculateMd5Hash(tempKey);

            if (hash.StartsWith("00000"))
                return result.ToString();
        }
    }

    public string SolveSecondPart(Input input)
    {
        var key = input.GetAsString();

        var result = 0;
        while (true)
        {
            result++;
            var tempKey = key + result;

            var hash = CalculateMd5Hash(tempKey);

            if (hash.StartsWith("000000"))
                return result.ToString();
        }
    }
        
    private string CalculateMd5Hash(string input)
    {
        var md5 = MD5.Create();
        var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        var hash = md5.ComputeHash(inputBytes);
            
        var sb = new StringBuilder();
        foreach (var t in hash)
        {
            sb.Append(t.ToString("X2"));
        }
        return sb.ToString();
    }
}