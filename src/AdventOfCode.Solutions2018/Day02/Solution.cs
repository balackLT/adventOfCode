using System.Collections.Generic;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2018.Day02;

public class Solution : ISolution
{
    public int Day { get; } = 2;

    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var ids = new List<string>();

        var doubleLetter = 0;
        var tripleLetter = 0;
        bool isDouble;
        bool isTriple;

        foreach (string line in lines)
        {
            var frequencies = new Dictionary<char, int>();

            isDouble = false;
            isTriple = false;

            foreach(var letter in line)
            {
                if (frequencies.ContainsKey(letter))
                    frequencies[letter]++;
                else frequencies[letter] = 1;
            }

            foreach(var frequency in frequencies)
            {
                if (frequency.Value == 2)
                    isDouble = true;
                if (frequency.Value == 3)
                    isTriple = true;
            }

            if (isDouble)
                doubleLetter++;
            if (isTriple)
                tripleLetter++;

            ids.Add(line);
        }

        return (doubleLetter * tripleLetter).ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var ids = new List<string>();

        foreach (string line in lines)
        {
            ids.Add(line);
        }

        var length = ids[0].Length;

        foreach (var id in ids)
        {
            foreach (var id2 in ids)
            {
                var commonLetters = "";

                for (int i = 0; i < length; i++)
                {
                    if (id2[i] == id[i])
                        commonLetters += id[i];
                }

                if (commonLetters.Length == length - 1)
                    return commonLetters;
            }
        }

        return 0.ToString();
    }

}