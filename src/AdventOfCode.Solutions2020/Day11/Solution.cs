using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2020.Day11;

public class Solution : ISolution
{
    public int Day { get; } = 11;

    public object SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var seatMap = new SeatMap(map, ' ');

        while (seatMap.UpdateSeats() > 0)
        {
                
        }

        var result = seatMap.InternalMap.Count(s => s.Value == '#');
            
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var seatMap = new SeatMap(map, ' ');

        while (seatMap.UpdateSeats2() > 0)
        {
        }

        var result = seatMap.InternalMap.Count(s => s.Value == '#');
            
        return result.ToString();
    }

    private class SeatMap : Map<char>
    {
        public SeatMap(Dictionary<Coordinate, char> map, char defaultLocation) : base(map, defaultLocation)
        {
            _maxX = InternalMap.Max(m => m.Key.X);
            _maxY = InternalMap.Max(m => m.Key.Y);
        }

        private readonly long _maxX;
        private readonly long _maxY;
            
        public int UpdateSeats()
        {
            var updatedSeats = 0;

            var updatedMap = new Dictionary<Coordinate, char>(InternalMap);
                
            foreach (var seat in InternalMap)
            {
                if (seat.Value == 'L' && CountAdjacent(seat.Key, '#') == 0)
                {
                    updatedMap[seat.Key] = '#';
                    updatedSeats++;
                }
                else if (seat.Value == '#' && CountAdjacent(seat.Key, '#') >= 4)
                {
                    updatedMap[seat.Key] = 'L';
                    updatedSeats++;
                }
            }

            InternalMap = updatedMap;

            return updatedSeats;
        }

        public int UpdateSeats2()
        {
            var updatedSeats = 0;

            var updatedMap = new Dictionary<Coordinate, char>(InternalMap);
                
            foreach (var seat in InternalMap)
            {
                if (seat.Value == 'L' && AreVisible(seat.Key, '#', 'L', 0))
                {
                    updatedMap[seat.Key] = '#';
                    updatedSeats++;
                }
                else if (seat.Value == '#' && AreVisible(seat.Key, '#',  'L',5))
                {
                    updatedMap[seat.Key] = 'L';
                    updatedSeats++;
                }
            }

            InternalMap = updatedMap;

            return updatedSeats;
        }
            
        private int CountAdjacent(Coordinate seat, char target)
        {
            var adjacent = seat
                .GetAdjacentWithDiagonals()
                .Where(c => c.X >= 0 &&
                            c.Y >= 0 &&
                            c.X <= _maxX &&
                            c.Y <= _maxY);

            var count = adjacent.Count(s => InternalMap[s] == target);
                
            return count;
        }
            
        private bool AreVisible(Coordinate seat, char target, char otherTarget, int targetCount)
        {
            var visibleCount = 0;
                
            foreach (var direction in Coordinate.ExtendedDirections)
            {
                var location = seat;
                    
                if (visibleCount > targetCount)
                    return false;
                    
                if (targetCount > 0 && visibleCount == targetCount)
                    return true;

                while (true)
                {
                    location += direction;

                    if (location.X < 0 ||
                        location.Y < 0 ||
                        location.X > _maxX ||
                        location.Y > _maxY)
                    {
                        break; // out of bounds
                    }

                    var item = InternalMap[location];
                        
                    if (item == target)
                    {
                        visibleCount++;
                        break;
                    }
                        
                    if (item == otherTarget)
                    {
                        break;
                    }
                }
            }

            return visibleCount == targetCount;
        }
    }
}