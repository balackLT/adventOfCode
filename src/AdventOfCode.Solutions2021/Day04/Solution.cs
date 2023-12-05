using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2021.Day04;

public class Solution : ISolution
{
    public int Day { get; } = 4;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var instructions = lines.First().Split(',').Select(int.Parse);

        var boards = ParseBoards(lines).ToList();
        
        foreach (var instruction in instructions)
        {
            foreach (var board in boards)
            {
                board.Mark(instruction);
                if (board.IsSolved())
                {
                    var result = board.Map.Where(c => !c.Value.IsMarked).Select(v => v.Value.Value).Sum();
                    result *= instruction;
                    return result.ToString();
                }
            }
        }

        return 0.ToString();
    }
    
    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var instructions = lines.First().Split(',').Select(int.Parse);

        var boards = ParseBoards(lines).ToList();
        
        foreach (var instruction in instructions)
        {
            foreach (var board in boards)
            {
                board.Mark(instruction);
                if (board.IsSolved() && boards.Count == 1)
                {
                    var result = board.Map.Where(c => !c.Value.IsMarked).Select(v => v.Value.Value).Sum();
                    result *= instruction;
                    return result.ToString();
                }
            }

            
            boards = boards.Where(b => !b.IsSolved()).ToList();
        }

        return 0.ToString();
    }

    private static IEnumerable<BingoBoard> ParseBoards(string[] lines)
    {
        var boards = new List<Map<BingoValue>>();

        Map<BingoValue> board = null;

        int y = 0;
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrEmpty(line))
            {
                if (board is not null)
                {
                    boards.Add(board);
                }

                board = new Map<BingoValue>(null);
                y = 0;
            }
            else
            {
                var numbers = line.Split().Where(c => !string.IsNullOrWhiteSpace(c)).Select(int.Parse);
                int x = 0;
                foreach (var number in numbers)
                {
                    board![new Coordinate(x, y)] = new BingoValue(number);
                    x++;
                }

                y++;
            }
        }
        boards.Add(board);

        return boards.Select(b => new BingoBoard(b));
    }

}