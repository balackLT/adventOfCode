using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using MoreLinq;

namespace AdventOfCode.Solutions2020.Day22;

public class Solution : ISolution
{
    public int Day { get; } = 22;

    public string SolveFirstPart(Input input)
    {
        var decks = ParseDecks(input);

        while (true)
        {
            if (!decks[0].Cards.TryPeek(out var card1) || !decks[1].Cards.TryPeek(out var card2))
                break;

            if (card1 > card2)
            {
                decks[0].Cards.Enqueue(decks[0].Cards.Dequeue());
                decks[0].Cards.Enqueue(decks[1].Cards.Dequeue());
            }
            else if (card2 > card1)
            {
                decks[1].Cards.Enqueue(decks[1].Cards.Dequeue());
                decks[1].Cards.Enqueue(decks[0].Cards.Dequeue());
            }
            else throw new Exception("wtf");
        }

        var winner = decks.Single(d => d.Cards.Count > 0);
        var result = winner.CountPoints();
            
        return result.ToString();
    }
        
    public string SolveSecondPart(Input input)
    {
        var decks = ParseDecks(input);

        var winner = PlayRecursive(decks[0], decks[1]);
        var result = winner.CountPoints();
            
        return result.ToString();
    }

    private Deck PlayRecursive(Deck deck1, Deck deck2)
    {
        var gameCache = new HashSet<string>();

        while (true)
        {
            if (!deck1.Cards.TryPeek(out var card1))
                return deck2;

            if (!deck2.Cards.TryPeek(out var card2))
                return deck1;

            var key = deck1.BuildKey() + ';' + deck2.BuildKey();

            if (!gameCache.Add(key))
                return deck1;

            if (deck1.Cards.Count > card1 && deck2.Cards.Count > card2)
            {
                var copy1 = deck1.DeepCopy();
                var copy2 = deck2.DeepCopy();

                copy1.Cards = new Queue<int>(copy1.Cards.Skip(1).Take(card1));
                copy2.Cards = new Queue<int>(copy2.Cards.Skip(1).Take(card2));

                var winner = PlayRecursive(copy1, copy2);
                if (winner.Number == 0)
                {
                    deck1.Cards.Enqueue(deck1.Cards.Dequeue());
                    deck1.Cards.Enqueue(deck2.Cards.Dequeue());
                }
                else if (winner.Number == 1)
                {
                    deck2.Cards.Enqueue(deck2.Cards.Dequeue());
                    deck2.Cards.Enqueue(deck1.Cards.Dequeue());
                }
                else throw new Exception("wtf");
            }
            else
            {
                if (card1 > card2)
                {
                    deck1.Cards.Enqueue(deck1.Cards.Dequeue());
                    deck1.Cards.Enqueue(deck2.Cards.Dequeue());
                }
                else if (card2 > card1)
                {
                    deck2.Cards.Enqueue(deck2.Cards.Dequeue());
                    deck2.Cards.Enqueue(deck1.Cards.Dequeue());
                }
                else throw new Exception("wtf");
            }
        }
    }
        
    private static List<Deck> ParseDecks(Input input)
    {
        return input
            .GetLines()
            .Split("")
            .Select((l, a) => new Deck
            {
                Number = a,
                Cards = new Queue<int>(l.Skip(1).Select(int.Parse))
            })
            .ToList();
    }
        
    private class Deck
    {
        public int Number { get; set; }
        public Queue<int> Cards { get; set; }

        public string BuildKey()
        {
            return string.Join(',', Cards);
        }

        public Deck DeepCopy()
        {
            return new Deck
            {
                Number = this.Number,
                Cards = new Queue<int>(this.Cards)
            };
        }
            
        public int CountPoints()
        {
            var cards = Cards.AsEnumerable().Reverse();

            var count = 1;
            var result = 0;
            foreach (var card in cards)
            {
                result += card * count;
                count++;
            }

            return result;
        }
    }
}