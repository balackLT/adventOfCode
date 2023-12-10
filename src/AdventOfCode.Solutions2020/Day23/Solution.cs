using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using MoreLinq;

namespace AdventOfCode.Solutions2020.Day23;

public class Solution : ISolution
{
    public int Day { get; } = 23;

    public object SolveFirstPart(Input input)
    {
        var list = input.GetAsString();
        // var list = "389125467";
            
        var cups = ParseCups(list);
        var maxId = cups.Max(c => c.Key);

        // Debug.Assert(cups.First().Value.GetOrder() == "89125467");

        var currentCup = cups.First().Value;
        for (int turn = 0; turn < 100; turn++)
        {
            var three = currentCup.TakeThree();
            var destinationCup = GetDestinationCup(currentCup, cups, maxId, three.Item2);
            destinationCup.AddThreeAfter(three.Item1);
            currentCup = currentCup.Next;
        }

        var result = cups[1].GetOrder();

        return result;
    }

    private static Cup GetDestinationCup(Cup currentCup, Dictionary<long, Cup> cups, long maxId, List<long> taken)
    {
        var nextCupId = currentCup.Number - 1;
        while (true)
        {
            if (nextCupId < 1)
                nextCupId = maxId;

            if (taken.Contains(nextCupId))
                nextCupId -= 1;
            else break;
        }

        var destinationCup = cups[nextCupId];
        return destinationCup;
    }

    public object SolveSecondPart(Input input)
    {
        var list = input.GetAsString();
        // var list = "389125467";
            
        var cups = ParseCups(list);
        long maxId = cups.Max(c => c.Key);
        var lastCup = cups.Last();
        var previousCup = new Cup
        {
            Number = maxId + 1,
        };
        cups[maxId + 1] = previousCup;
        lastCup.Value.Next = previousCup;
        for (long i = maxId + 2; i <= 1000000; i++)
        {
            var cup = new Cup
            {
                Number = i
            };
            previousCup.Next = cup;
            previousCup = cup;
            cups.Add(cup.Number, cup);
        }

        var last = cups.Last();
        last.Value.Next = cups.First().Value;
            
        maxId = cups.Max(c => c.Key);
            
        var currentCup = cups.First().Value;
        for (long turn = 0; turn < 10000000; turn++)
        {
            var three = currentCup.TakeThree();
            var destinationCup = GetDestinationCup(currentCup, cups, maxId, three.Item2);
            destinationCup.AddThreeAfter(three.Item1);
            currentCup = currentCup.Next;
        }

        long result = cups[1].Next.Number * cups[1].Next.Next.Number;

        return result.ToString();
    }

    private static Dictionary<long, Cup> ParseCups(string list)
    {
        var cups = list
            .Select(c => long.Parse(c.ToString()))
            .Select(c => new KeyValuePair<long, Cup>(c, new Cup {Number = c}))
            .ToDictionary();

        var previousCup = cups.Last().Value;
        foreach (var cup in cups)
        {
            previousCup.Next = cup.Value;
            previousCup = cup.Value;
        }

        return cups;
    }
        
    private class Cup
    {
        public long Number { get; init; }
        public Cup Next { get; set; }

        // we only need the pointer to the first of the three
        public (Cup, List<long>) TakeThree()
        {
            var first = Next;
            var second = first.Next;
            var third = second.Next;

            var taken = new List<long>
            {
                first.Number,
                second.Number,
                third.Number
            };

            this.Next = third.Next;
                
            return (first, taken);
        }

        public void AddThreeAfter(Cup threeCupsPointer)
        {
            var temp = Next;

            this.Next = threeCupsPointer;
            var second = threeCupsPointer.Next;
            var third = second.Next;
            third.Next = temp;
        }

        public string GetOrder()
        {
            var result = "";

            var current = this;
            while (true)
            {
                var next = current.Next;
                if (next == this)
                    break;

                result += next.Number;
                current = next;
            }

            return result;
        }
    }
}