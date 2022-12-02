using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2022.Day02;

public class Solution : ISolution
{
    public int Year => 2022;
    public int Day => 1;
    public bool PartOneSolved => true;
    public bool PartTwoSolved => true;

    private static readonly Dictionary<string, Throw> Map = new()
    {
        {"A", Throw.ROCK},
        {"B", Throw.PAPER},
        {"C", Throw.SCISSORS},
        {"X", Throw.ROCK},
        {"Y", Throw.PAPER},
        {"Z", Throw.SCISSORS}
    };
    
    public string SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var matches = lines
            .Select(l => l.Split().Select(t => Map[t]).ToList());

        var score = 0;
        
        foreach (var match in matches)
        {
            score += match[1] switch
            {
                Throw.ROCK => 1,
                Throw.PAPER => 2,
                Throw.SCISSORS => 3,
                _ => throw new ArgumentOutOfRangeException()
            };

            score += Play(match[1], match[0]) switch
            {
                State.WIN => 6,
                State.LOSS => 0,
                State.DRAW => 3,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return score.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var lines = input.GetLines();

        var score = 0;
        
        foreach (var line in lines)
        {
            var splitLine = line.Split();

            var state = splitLine[1] switch
            {
                "X" => State.LOSS,
                "Y" => State.DRAW,
                "Z" => State.WIN,
                _ => throw new ArgumentOutOfRangeException()
            };

            score += state switch
            {
                State.WIN => 6,
                State.LOSS => 0,
                State.DRAW => 3,
                _ => throw new ArgumentOutOfRangeException()
            };

            var myThrow = ThrowByState(state, Map[splitLine[0]]);
            
            score += myThrow switch
            {
                Throw.ROCK => 1,
                Throw.PAPER => 2,
                Throw.SCISSORS => 3,
                _ => throw new ArgumentOutOfRangeException()
            };
        }    
        
        return score.ToString();
    }

    private static Throw ThrowByState(State player, Throw opponent)
    {
        return player switch
        {
            State.WIN => opponent switch
            {
                Throw.ROCK => Throw.PAPER,
                Throw.PAPER => Throw.SCISSORS,
                Throw.SCISSORS => Throw.ROCK,
                _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
            },
            State.LOSS => opponent switch
            {
                Throw.ROCK => Throw.SCISSORS,
                Throw.PAPER => Throw.ROCK,
                Throw.SCISSORS => Throw.PAPER,
                _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
            },
            State.DRAW => opponent switch
            {
                Throw.ROCK => Throw.ROCK,
                Throw.PAPER => Throw.PAPER,
                Throw.SCISSORS => Throw.SCISSORS,
                _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
        };
    }
    
    private static State Play(Throw player, Throw opponent)
    {
        return player switch
        {
            Throw.ROCK => opponent switch
            {
                Throw.ROCK => State.DRAW,
                Throw.PAPER => State.LOSS,
                Throw.SCISSORS => State.WIN,
                _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
            },
            Throw.PAPER => opponent switch
            {
                Throw.ROCK => State.WIN,
                Throw.PAPER => State.DRAW,
                Throw.SCISSORS => State.LOSS,
                _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
            },
            Throw.SCISSORS => opponent switch
            {
                Throw.ROCK => State.LOSS,
                Throw.PAPER => State.WIN,
                Throw.SCISSORS => State.DRAW,
                _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
        };
    }

    private enum State
    {
        WIN,
        LOSS,
        DRAW
    }

    private enum Throw
    {
        ROCK,
        PAPER,
        SCISSORS
    }
}