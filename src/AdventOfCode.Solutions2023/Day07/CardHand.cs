namespace AdventOfCode.Solutions2023.Day07;

public class CardHand : IComparable<CardHand>
{
    public readonly string OriginalHand;
    public readonly int Bet;
    public readonly List<int> ParsedHand = new();
    public readonly Dictionary<int, int> CardCounts = new();
    
    public CardHand(string hand, int bet)
    {
        OriginalHand = hand;
        Bet = bet;
        
        foreach (char c in OriginalHand)
        {
            if (char.IsNumber(c))
            {
                ParsedHand.Add(int.Parse(c.ToString()));
            }
            else switch (c)
            {
                case 'T':
                    ParsedHand.Add(10);
                    break;
                case 'J':
                    ParsedHand.Add(11);
                    break;
                case 'Q':
                    ParsedHand.Add(12);
                    break;
                case 'K':
                    ParsedHand.Add(13);
                    break;
                case 'A':
                    ParsedHand.Add(14);
                    break;
            }
        }
        
        foreach (int card in ParsedHand)
        {
            CardCounts[card] = CardCounts.TryGetValue(card, out int count) ? count + 1 : 1;
        }
    }

    public int Strength()
    {
        if (CardCounts.Any(c => c.Value == 5))
        {
            return 7;
        }
        
        if (CardCounts.Any(c => c.Value == 4))
        {
            return 6;
        }
        
        // full house
        if (CardCounts.Any(c => c.Value == 3) && CardCounts.Any(c => c.Value == 2))
        {
            return 5;
        }
        
        if (CardCounts.Any(c => c.Value == 3))
        {
            return 4;
        }
        
        //two pair
        if (CardCounts.Count(c => c.Value == 2) == 2)
        {
            return 3;
        }
        
        if (CardCounts.Any(c => c.Value == 2))
        {
            return 2;
        }

        return 1;
    }

    public int CompareTo(CardHand? other)
    {
        if (Strength() > other!.Strength())
        {
            return 1;
        }

        if (Strength() < other!.Strength())
        {
            return -1;
        }

        for (int i = 0; i < 5; i++)
        {
            if (ParsedHand[i] > other.ParsedHand[i])
            {
                return 1;
            }
            
            if (ParsedHand[i] < other.ParsedHand[i])
            {
                return -1;
            }
        }
        
        return 0;
    }
}