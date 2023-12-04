using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day04;

public class Solution : ISolution
{
    public int Day { get; } = 4;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();
        
        List<Card> cards = ParseCards(lines).ToList();

        var result = 0;

        foreach (var card in cards) 
        {
            var winners = card.WinningNumbers.Intersect(card.PlayedNumbers).ToList();
            if (winners.Count > 0)
            {
                result += (int)Math.Pow(2, winners.Count - 1);
            }
        }
        
        return result.ToString();
    }

    private static IEnumerable<Card> ParseCards(IEnumerable<string> lines)
    {
        return 
            from line in lines 
            select line.Split(":") 
            into split 
            let cardNumber = int.Parse(split[0][4..]) 
            let numberLists = split[1].Split('|', StringSplitOptions.RemoveEmptyEntries) 
            let winningNumbers = numberLists[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() 
            let numbers = numberLists[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() 
            select new Card(cardNumber, winningNumbers, numbers);
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();
        
        List<Card> cards = ParseCards(lines).ToList();
        
        var instances = new Dictionary<int, int>();
        foreach (var card in cards)
        {
            instances[card.CardNumber] = 1;
        }

        foreach (var card in cards) 
        {
            var winners = card.WinningNumbers.Intersect(card.PlayedNumbers).ToList();
            if (winners.Count > 0)
            {
                for(var i = 1; i <= winners.Count; i++)
                {
                    instances[card.CardNumber + i] += instances[card.CardNumber];
                }
            }
        }
        
        return instances.Values.Sum().ToString();
    }

    private record Card(int CardNumber, List<int> WinningNumbers, List<int> PlayedNumbers);
}