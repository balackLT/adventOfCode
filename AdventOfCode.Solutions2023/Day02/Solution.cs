using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2023.Day02;

public class Solution : ISolution
{
    public int Day { get; } = 2;
    
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        const int redLimit = 12;
        const int greenLimit = 13;
        const int blueLimit = 14;
        
        var games = ParseGames(lines);

        var result = 0;
        foreach (Game game in games)
        {
            var valid = true;
            foreach (Round gameRound in game.Rounds)
            {
                if (gameRound.Blue > blueLimit || gameRound.Red > redLimit || gameRound.Green > greenLimit)
                {
                    valid = false;
                }
            }
            
            if (valid)
                result += game.Id;
        }
        
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines();
        
        var games = ParseGames(lines);

        var result = 0;
        foreach (Game game in games)
        {
            var minGreen = game.Rounds.Max(r => r.Green);
            var minRed = game.Rounds.Max(r => r.Red);
            var minBlue = game.Rounds.Max(r => r.Blue);
            
            result += minGreen * minRed * minBlue;
        }
        
        return result.ToString();
    }
    
    private static IEnumerable<Game> ParseGames(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var splitLine = line.Split(':');
            var gameNumber = int.Parse(splitLine[0][5..]);

            var game = new Game
            {
                Id = gameNumber
            };
            var rounds = splitLine[1].Split(';');

            foreach (var round in rounds)
            {
                var newRound = new Round();
                var items = round.Split(',');
                foreach (var item in items)
                {
                    var token = item.Trim().Split(' ');
                    var count = int.Parse(token[0]);
                    switch (token[1])
                    {
                        case "blue":
                            newRound.Blue = count;
                            break;
                        case "green":
                            newRound.Green = count;
                            break;
                        case "red":
                            newRound.Red = count;
                            break;
                    }
                }

                game.Rounds.Add(newRound);
            }

            yield return game;
        }
    }
}