using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using AdventOfCode.Executor;
using AdventOfCode.Solutions2019.Shared.Map;

namespace AdventOfCode.Solutions2019.Day10
{
    public class Solution : ISolution
    {
        private const char ASTEROID = '#';
        private const char SPACE = '.';
        
        public int Day { get; } = 10;

        private Dictionary<Coordinate, char> _map;
        
        public string SolveFirstPart(Input input)
        {
            var inputMap = input.GetAsMap();

            var map = inputMap
                .Select(i => new KeyValuePair<Coordinate, char>(new Coordinate(i.Key.X, i.Key.Y), i.Value))
                .ToDictionary(i => i.Key, i => i.Value);
            
            _map = map;

            Debug.Assert(GetPointsBetween(new Coordinate(1,0), new Coordinate(4,0)).Count == 2);
            Debug.Assert(GetPointsBetween(new Coordinate(0,1), new Coordinate(0,4)).Count == 2);
            Debug.Assert(GetPointsBetween(new Coordinate(1,3), new Coordinate(4,0)).Count == 2);
            Debug.Assert(GetPointsBetween(new Coordinate(1,0), new Coordinate(3,4)).Count == 1);
            Debug.Assert(GetPointsBetween(new Coordinate(2,15), new Coordinate(6,15)).Count == 3);

            var asteroidCoordinates = map.Where(c => c.Value == ASTEROID).Select(c => c.Key).ToList();
            
            var result = asteroidCoordinates.Max(a => CountVisibleAsteroids(a, asteroidCoordinates));

            return result.ToString();
        }

        private int CountVisibleAsteroids(Coordinate asteroid, IEnumerable<Coordinate> allAsteroids)
        {
            var result = allAsteroids.Where(a => a != asteroid)
                .Count(o => GetPointsBetween(o, asteroid).Count(p => _map[p] == ASTEROID) == 0);
            return result;
        }
        
        private List<Coordinate> GetPointsBetween(Coordinate one, Coordinate two)
        {
            var result = new List<Coordinate>();

            if (one == two)
                return result;

            var directionX = (one.X < two.X) ? 1 : -1;
            var directionY = (one.Y < two.Y) ? 1 : -1;
            
            // Trivial cases
            if (Math.Abs(one.X - two.X) == 0)
            {
                for (var y = one.Y + directionY; y != two.Y; y+= directionY)
                {
                    var coord = new Coordinate(one.X, y);
                    result.Add(coord);
                }
                return result;
            }
            
            if (Math.Abs(one.Y - two.Y) == 0)
            {
                for (var x = one.X + directionX; x != two.X; x+= directionX)
                {
                    var coord = new Coordinate(x, one.Y);
                    result.Add(coord);
                }
                return result;
            }
            
            for (var y = one.Y; y != two.Y; y+= directionY)
            {
                for (var x = one.X; x != two.X; x += directionX)
                {
                    if (new Coordinate(x, y) == one || new Coordinate(x, y) == two)
                        continue;

                    var crossProduct = (y - one.Y) * (two.X - one.X) - (x - one.X) * (two.Y - one.Y);
                    if (crossProduct == 0)
                        result.Add(new Coordinate(x, y));
                }
            }

            return result;
        }

        public string SolveSecondPart(Input input)
        {
            var inputMap = input.GetAsMap();

            var map = inputMap
                .Select(i => new KeyValuePair<Coordinate, char>(new Coordinate(i.Key.X, i.Key.Y), i.Value))
                .ToDictionary(i => i.Key, i => i.Value);
            
            var asteroidCoordinates = map.Where(c => c.Value == ASTEROID).Select(c => c.Key);
            
            // DEPLOY LASER STATION PEW PEW PEW
            var station = map
                .Where(c => c.Value == ASTEROID)
                .Select(c => c.Key)
                .Select(a => (Location: a, Count: CountVisibleAsteroids(a, asteroidCoordinates)))
                .OrderByDescending(a => a.Count)
                .First().Location;

            var destroyedCount = 0;
            
            while (true)
            {
                // ACQUIRE REMAINING TARGETS 
                asteroidCoordinates = map.Where(c => c.Value == ASTEROID).Select(c => c.Key);
                
                // CALIBRATE LAZORS
                var targets = asteroidCoordinates
                    .Where(a => a != station)
                    .Where(a => GetPointsBetween(a, station).Count(p => map[p] == ASTEROID) == 0)
                    .Select(a => (Asteroid: a, Angle: Coordinate.AngleBetween(station, a)))
                    .Select(t => (t.Asteroid, Angle: t.Angle < -90 ? t.Angle += 360 : t.Angle)) // magical LAZOR "rotation", don't do this at home, only elves can do this
                    .OrderBy(t => t.Angle)
                    .ToList();
                
                // DESTROY DESTROY DESTROY PEW PEW PEW PEW PEW
                foreach (var target in targets)
                {
                    destroyedCount++;
                    if (destroyedCount == 200)
                        return (target.Asteroid.X * 100 + target.Asteroid.Y).ToString();

                    map[target.Asteroid] = SPACE;
                }
            }
        }
    }
}