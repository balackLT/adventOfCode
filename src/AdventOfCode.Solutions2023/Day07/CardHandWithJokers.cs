namespace AdventOfCode.Solutions2023.Day07;

public class CardHandWithJokers : IComparable<CardHandWithJokers>
{
    public readonly string OriginalHand;
    public readonly int Bet;
    public readonly List<int> ParsedHand = new();
    public readonly Dictionary<int, int> CardCounts = new();
    
    public CardHandWithJokers(string hand, int bet)
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
                    ParsedHand.Add(1);
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
            // cannot be upgraded, always best
            
            return 7;
        }
        
        if (CardCounts.Any(c => c.Value == 4))
        {
            // Joker replaces missing 5th card, e.g. 4444J -> 44444
            // hmm.. what if JJJJ5? ok, means we can replace all Js with 5s and 5 of a kind still
            if (CardCounts.Any(c => c.Key == 1))
                return 7;
            
            return 6;
        }
        
        // full house
        if (CardCounts.Any(c => c.Value == 3) && CardCounts.Any(c => c.Value == 2))
        {
            // can always be upgraded to 5 of a kind if joker, no case for 4 of a kind
            // e.g. 333JJ -> 33333
            // e.g. JJJ33 -> 33333
            if (CardCounts.Any(c => c.Key == 1))
                return 7;
            
            return 5;
        }
        
        // three of a kind
        if (CardCounts.Any(c => c.Value == 3))
        {
            // can be upgraded to five of a kind if exactly double joker
            // e.g. 333JJ -> 33333
            if (CardCounts.Any(c => c.Key == 1 && c.Value == 2))
                return 7;
            
            // can be upgraded to 4 of a kind if triple or single joker
            // e.g. 333J4 -> 33334
            // e.g. JJJ34 -> 33334
            if (CardCounts.Any(c => c.Key == 1))
                return 6;
            
            return 4;
        }
        
        //two pair
        if (CardCounts.Count(c => c.Value == 2) == 2)
        {
            // can be upgraded to 4 of a kind if any pair is jokers
            // e.g. 33JJ4 -> 33334
            if (CardCounts.Any(c => c.Key == 1 && c.Value == 2))
                return 6;
            
            // can be upgraded to full house if fifth card is a single joker
            // e.g. 3344J -> 33444
            if (CardCounts.Any(c => c.Key == 1 && c.Value == 1))
                return 5;
            
            // cannot be upgraded to three of kind (I think) cuz then we'd get either four of a kind or FH
            
            return 3;
        }
        
        if (CardCounts.Any(c => c.Value == 2))
        {
            // can be upgraded to three of a kind if any joker
            // e.g. 22J34 -> 22234
            // e.g. JJ234 -> 22234
            if (CardCounts.Any(c => c.Key == 1))
                return 4;
            
            // can be upgraded to two pair if joker e.g. 22J34 -> 22334
            // nevermind, then we can always upgrade to three of a kind
            
            return 2;
        }
        
        // can be upgraded to single pair if joker e.g. 2J345 -> 22345
        if (CardCounts.Any(c => c.Key == 1))
            return 2;
        
        return 1;
    }

    public int CompareTo(CardHandWithJokers? other)
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