using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2020.Day05;

public class Solution : ISolution
{
    public int Day { get; } = 5;

    public object SolveFirstPart(Input input)
    {
        var seatCodes = input.GetLinesAsList().Select(l => new Queue<char>(l));

        var result = seatCodes
            .Select(GetSeat)
            .Max(c => c.Row * 8 + c.Column);
            
        return result.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var seatCodes = input.GetLinesAsList().Select(l => new Queue<char>(l));

        var result = seatCodes
            .Select(GetSeat)
            .Select(c => c.Row * 8 + c.Column)
            .OrderBy(c => c)
            .ToList();

        var allCodes = Enumerable.Range(result.Min(), result.Max());

        var mySeat = allCodes.Except(result).FirstOrDefault();
            
        return mySeat.ToString();
    }

    private Seat GetSeat(Queue<char> seatCode)
    {
        var minRow = 0;
        var maxRow = 127;
        var minColumn = 0;
        var maxColumn = 7;
            
        while (seatCode.TryDequeue(out var current))
        {
            switch (current)
            {
                case 'F':
                    maxRow -= (maxRow - minRow + 1) / 2;
                    break;
                case 'B':
                    minRow += (maxRow - minRow + 1) / 2;
                    break;
                case 'L':
                    maxColumn -= (maxColumn - minColumn + 1) / 2;
                    break;
                case 'R':
                    minColumn += (maxColumn - minColumn + 1) / 2;
                    break;
            }
        }

        if (minRow != maxRow || minColumn != maxColumn)
            throw new Exception("wtf");

        return new Seat(maxRow, maxColumn);
    }

    private record Seat (int Row, int Column);
}