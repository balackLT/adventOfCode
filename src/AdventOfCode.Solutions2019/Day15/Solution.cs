using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Computer;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2019.Day15;

public class Bot
{
    private readonly Computer _brain;
    private Coordinate _location = new Coordinate(0, 0);
    private readonly Map<char> _map = new Map<char>(UNKNOWN);
    public Coordinate GeneratorLocation;
    private int _distanceTraveled = 0;
    private readonly Map<int> _distances = new Map<int>(99999);

    private readonly Dictionary<int, Coordinate> _directionMap = new Dictionary<int, Coordinate>
    {
        {1, Coordinate.North},
        {2, Coordinate.South},
        {3, Coordinate.West},
        {4, Coordinate.East}
    };

    public const char STARTING_LOCATION = '0';
    public const char EMPTY = '?';
    public const char EXPLORED = '.';
    public const char WALL = '█';
    public const char UNKNOWN = ' ';
    public const char OXYGEN = 'O';

    public Bot(IEnumerable<long> program)
    {
        _brain = new Computer(program);
        _map[new Coordinate(0, 0)] = STARTING_LOCATION;
    }

    private int Move(int direction)
    {
        _brain.Run(direction);
        Debug.Assert(_brain.Output.Count == 1);
        var result = (int)_brain.GetOutput();

        switch (result)
        {
            case 0:
                _map[_location + _directionMap[direction]] = WALL; 
                break;
            case 1:
                _location += _directionMap[direction];
                    
                _distanceTraveled++;
                if (_distances[_location] < _distanceTraveled)
                    _distanceTraveled = _distances[_location];
                else if (_distances[_location] >= _distanceTraveled)
                    _distances[_location] = _distanceTraveled;

                _map[_location] = EMPTY;
                break;
            case 2:
                _location += _directionMap[direction];
                    
                _distanceTraveled++;
                if (_distances[_location] < _distanceTraveled)
                    _distanceTraveled = _distances[_location];
                else if (_distances[_location] >= _distanceTraveled)
                    _distances[_location] = _distanceTraveled;
                    
                _map[_location] = EMPTY;
                GeneratorLocation = _location;
                break;
            default:
                throw new Exception("Unexpected response");
        }
            
        return result;
    }

    public void Draw()
    {
        var temp = _map[_location];
        _map[_location] = 'D';
        _map[GeneratorLocation] = 'G';
        _map.PrintMap();
        _map[_location] = temp;
    }

    public int DistanceTo(Coordinate to)
    {
        return _distances[to];
    }

    public void FillMap()
    {
        while (true)
        {
            if (_map[_location + Coordinate.North] == UNKNOWN)
            {
                Move(1);
                continue;
            }
                
            if (_map[_location + Coordinate.South] == UNKNOWN)
            {
                Move(2);
                continue;
            }
                
            if (_map[_location + Coordinate.West] == UNKNOWN)
            {
                Move(3);
                continue;
            }
                
            if (_map[_location + Coordinate.East] == UNKNOWN)
            {
                Move(4);
                continue;
            }

            _map[_location] = EXPLORED;
                
            if (_map[_location + Coordinate.North] == EMPTY)
            {
                Move(1);
                continue;
            }
                
            if (_map[_location + Coordinate.South] == EMPTY)
            {
                Move(2);
                continue;
            }
                
            if (_map[_location + Coordinate.West] == EMPTY)
            {
                Move(3);
                continue;
            }
                
            if (_map[_location + Coordinate.East] == EMPTY)
            {
                Move(4);
                continue;
            }
                
            break;
        }
    }

    public int FloodFill(Coordinate start)
    {
        // Destructive, slow and ugly
        var time = 0;
        _map[new Coordinate(0, 0)] = EXPLORED;
        _map[start] = OXYGEN;
            
        while (true)
        {
            if (_map.InternalMap.All(t => t.Value != EXPLORED))
                break;
                
            time++;

            foreach (var tile in _map.InternalMap.Where(t => t.Value == OXYGEN).ToList())
            {
                foreach (var direction in Coordinate.Directions.Where(direction => _map[tile.Key + direction] == EXPLORED))
                {
                    _map[tile.Key + direction] = OXYGEN;
                }
            }
        }
            
        return time;
    }
}
    
public class Solution : ISolution
{
    public int Day { get; } = 15;
        
    public string SolveFirstPart(Input input)
    {
        var program = input.GetLineAsLongArray();
            
        var bot = new Bot(program);
        bot.FillMap();
        // bot.Draw();
        var result = bot.DistanceTo(bot.GeneratorLocation);
            
        return result.ToString();
    }

    public string SolveSecondPart(Input input)
    {
        var program = input.GetLineAsLongArray();
            
        var bot = new Bot(program);
        bot.FillMap();
        var result = bot.FloodFill(bot.GeneratorLocation);
            
        return result.ToString();
    }
}