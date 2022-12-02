using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using Input = AdventOfCode.Executor.Input;

namespace AdventOfCode.Solutions2020.Day25;

public class Solution : ISolution
{
    public int Day { get; } = 25;
        
    private const long Divisor = 20201227;

    public string SolveFirstPart(Input input)
    {
        var keys = input.GetLines().Select(long.Parse).ToList();

        Debug.Assert(GuessLoopSize(5764801) == 8);
        Debug.Assert(Transform(17807724, 8) == 14897079);

        var cardLoopSize = GuessLoopSize(keys[0]);
        // var doorLoopSize = GuessLoopSize(keys[1]);

        var result = Transform(keys[1], cardLoopSize);

        return result.ToString();
    }
        
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        return 0.ToString();
    }

    private int GuessLoopSize(long key)
    {
        var loopSize = 0;
        long value = 1;

        while (true)
        {
            loopSize++;

            value = value * 7 % Divisor;

            if (value == key)
                return loopSize;
        }
    }

    private long Transform(long subject, int loopSize)
    {
        long value = 1;
            
        for (int i = 0; i < loopSize; i++)
        {
            value *= subject;
            value %= Divisor;
        }

        return value;
    }
}