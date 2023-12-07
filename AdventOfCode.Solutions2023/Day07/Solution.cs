using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day07;

public class Solution : ISolution
{
    public int Day { get; } = 7;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var hands = lines
            .Select(l => new CardHand(l.Split()[0], int.Parse(l.Split()[1])))
            .ToList();

        var orderedHands = hands.OrderDescending().Reverse();

        var result = 0;

        var i = 1;
        foreach (var orderedHand in orderedHands)
        {
            result += orderedHand.Bet * i;
            i++;
        }
        
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var hands = lines
            .Select(l => new CardHandWithJokers(l.Split()[0], int.Parse(l.Split()[1])))
            .ToList();

        var orderedHands = hands.OrderDescending().Reverse();

        var result = 0;

        var i = 1;
        foreach (var orderedHand in orderedHands)
        {
            result += orderedHand.Bet * i;
            i++;
        }
        
        return result.ToString();
    }
}