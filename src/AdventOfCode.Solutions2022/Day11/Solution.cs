using System.Text.RegularExpressions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day11;

public class Solution : ISolution
{
    public int Day { get; } = 11;

    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var monkeys = ParseMonkeys(lines);

        for (int i = 0; i < 20; i++)
        {
            foreach (var currentMonkey in monkeys)
            {
                while (currentMonkey.Items.TryDequeue(out var item))
                {
                    currentMonkey.MonkeyBusiness++;
                    var level = item;

                    switch (currentMonkey.Operation)
                    {
                        case Operation.SQUARE:
                            level *= level;
                            break;
                        case Operation.ADD:
                            level += currentMonkey.OperationAmount;
                            break;
                        case Operation.MULTIPLY:
                            level *= currentMonkey.OperationAmount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    level /= 3;

                    if (level % currentMonkey.Test == 0)
                    {
                        monkeys[currentMonkey.True].Items.Enqueue(level);
                    }
                    else
                    {
                        monkeys[currentMonkey.False].Items.Enqueue(level);
                    }
                }
            }
        }

        var result = monkeys
            .OrderByDescending(m => m.MonkeyBusiness)
            .Take(2)
            .Select(m => m.MonkeyBusiness)
            .ToList();
        
        return (result[0] * result[1]).ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var monkeys = ParseMonkeys(lines);

        var factor = monkeys.Aggregate(1L, (current, monkey) => current * monkey.Test);

        for (int i = 0; i < 10_000; i++)
        {
            foreach (var currentMonkey in monkeys)
            {
                while (currentMonkey.Items.TryDequeue(out var item))
                {
                    currentMonkey.MonkeyBusiness++;
                    var level = item;

                    switch (currentMonkey.Operation)
                    {
                        case Operation.SQUARE:
                            level *= level;
                            break;
                        case Operation.ADD:
                            level += currentMonkey.OperationAmount;
                            break;
                        case Operation.MULTIPLY:
                            level *= currentMonkey.OperationAmount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    level %= factor;

                    if (level % currentMonkey.Test == 0)
                    {
                        monkeys[currentMonkey.True].Items.Enqueue(level);
                    }
                    else
                    {
                        monkeys[currentMonkey.False].Items.Enqueue(level);
                    }
                }
            }
        }

        var result = monkeys
            .OrderByDescending(m => m.MonkeyBusiness)
            .Take(2)
            .Select(m => m.MonkeyBusiness)
            .ToList();
        
        return (result[0] * result[1]).ToString();
    }
    
    private static List<Monkey> ParseMonkeys(string[] lines)
    {
        var digitRegex = new Regex(@"\d+", RegexOptions.Compiled);

        var monkeys = new List<Monkey>();
        Monkey monkey = new Monkey();
        foreach (var line in lines)
        {
            if (line.Trim().StartsWith("Starting"))
            {
                monkey = new Monkey
                {
                    Items = new Queue<long>(digitRegex.Matches(line).Select(m => long.Parse(m.Value)))
                };
            }
            else if (line.Trim().StartsWith("Operation"))
            {
                var ops = line.Split().Reverse().Take(2).ToList();
                var op = ops[0];
                if (op == "old")
                    monkey.Operation = Operation.SQUARE;
                else
                {
                    monkey.OperationAmount = int.Parse(op);
                    if (ops[1] == "*")
                        monkey.Operation = Operation.MULTIPLY;
                    else
                        monkey.Operation = Operation.ADD;
                }
            }
            else if (line.Trim().StartsWith("Test"))
            {
                var test = int.Parse(digitRegex.Match(line).Value);
                monkey.Test = test;
            }
            else if (line.Trim().StartsWith("If true"))
            {
                var target = int.Parse(digitRegex.Match(line).Value);
                monkey.True = target;
            }
            else if (line.Trim().StartsWith("If false"))
            {
                var target = int.Parse(digitRegex.Match(line).Value);
                monkey.False = target;

                monkeys.Add(monkey);
            }
        }

        return monkeys;
    }
}