using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions2019.Shared.Computer;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2019.Day13;

public class Arcade
{
    private readonly Computer _processor;
    private readonly Map<int> _map = new Map<int>(0);
    public int Score;

    private readonly Dictionary<int, char> _decoder = new Dictionary<int, char>
    {
        {0, ' '},
        {1, '#'},
        {2, '█'},
        {3, '_'},
        {4, 'O'}
    };

    public Arcade(IEnumerable<long> program)
    {
        _processor = new Computer(program);
    }

    public void Run()
    {
        _processor.SetMemoryAddress(0, 2);

        var nextInput = 1;
        var previousBallLocation = new Coordinate(0, 0);
        var previousBallPrediction = new Coordinate(0,0);
            
        Console.Clear();
        var iteration = 0;
            
        while (_processor.State != State.FINISHED)
        {
            iteration++;
            _processor.Run(nextInput);

            while (_processor.Output.Count > 0)
            {
                var x = (int) _processor.GetOutput();
                var y = (int) _processor.GetOutput();
                var type = (int) _processor.GetOutput();

                if (x == -1 && y == 0)
                    Score = type;
                else _map[new Coordinate(x, y)] = type;
            }

            var paddleLocation = _map.InternalMap.Single(m => m.Value == 3).Key;
            var ballLocation = _map.InternalMap.Single(m => m.Value == 4).Key;
                
            var paddleLocationY = paddleLocation.Y;
            var ballLocationY = ballLocation.Y;
            var ballVelocityY = ballLocationY > previousBallLocation.Y ? 1 : -1;

            var paddleLocationX = paddleLocation.X;
            var ballLocationX = ballLocation.X;
            var ballVelocityX = ballLocationX > previousBallLocation.X ? 1 : -1;
            var ballVelocity = new Coordinate(ballVelocityX, ballVelocityY);
                
//                if (ballVelocity.Y == 1 && _map[new Coordinate(ballLocationX, ballLocation.Y + 1)] != 0)
//                    ballVelocity.Y = -1;
//                if (ballVelocity.Y == -1 && _map[new Coordinate(ballLocationX, ballLocation.Y - 1)] != 0)
//                    ballVelocity.Y = 1;
//                if (ballVelocity.X == 1 && _map[new Coordinate(ballLocationX + 1, ballLocation.Y)] != 0)
//                    ballVelocity.X = -1;
//                if (ballVelocity.X == -1 && _map[new Coordinate(ballLocationX - 1, ballLocation.Y)] != 0)
//                    ballVelocity.X = 1;

            var predictedBallLocation = ballLocation + ballVelocity;
                
            if (predictedBallLocation.X < paddleLocationX)
                nextInput = -1;
            else if (predictedBallLocation.X > paddleLocationX)
                nextInput = 1;

            if (ballLocationY == paddleLocationY - 1 && ballLocationX == paddleLocationX)
                nextInput = 0;
                
            Console.SetCursorPosition(0, 0);
            _map.PrintMap(_decoder);
                
            Console.WriteLine($"Score: {Score}, Iteration: {iteration}");
            Console.WriteLine($"{previousBallPrediction == ballLocation}, previousBallPrediction: {previousBallPrediction}");
            Console.WriteLine($"Ball: {ballLocation.ToString()}, Velocity: {ballVelocity}, Paddle: {paddleLocation.ToString()}");
            Console.WriteLine($"predictedBallLocation: {predictedBallLocation.ToString()}, nextInput: {nextInput}");
            //Console.ReadLine();
            //Thread.Sleep(100);
                
            previousBallPrediction = predictedBallLocation;
            previousBallLocation = ballLocation;
        }
    }
        
    public void RunDemo()
    {
        while (_processor.State != State.FINISHED)
        {
            _processor.Run();

            while (_processor.Output.Count > 0)
            {
                var x = (int) _processor.GetOutput();
                var y = (int) _processor.GetOutput();
                var type = (int) _processor.GetOutput();

                _map[new Coordinate(x, y)] = type;
            }
        }
    }

    public int CountTiles(int type)
    {
        return _map.InternalMap.Count(t => t.Value == type);
    }
}