using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day16;

public class Solution : ISolution
{
    private record CoordinateWithDirection(Coordinate Coordinate, Coordinate Direction);
    
    public object SolveFirstPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        List<Beam> beams = [new Beam(new Coordinate(-1, 0), Coordinate.Right)];
        int energizedCount = CalculateEnergization(beams, map);

        return energizedCount;
    }

    private static int CalculateEnergization(List<Beam> beams, Dictionary<Coordinate, char> map)
    {
        HashSet<CoordinateWithDirection> energized = [];

        while (beams.Any(b => b.Active))
        {
            var activeBeams = beams.Where(b => b.Active).ToList();
            foreach (var beam in activeBeams)
            {
                var next = beam.Location + beam.Direction;
                if (map.TryGetValue(next, out var tile))
                {
                    var coordinateWithDirection = new CoordinateWithDirection(next, beam.Direction);
                    if (energized.Contains(coordinateWithDirection))
                    {
                        beam.Active = false;
                        continue;
                    }
                    
                    energized.Add(new CoordinateWithDirection(next, beam.Direction));
                    beam.Location = next;
                    
                    if (tile == '\\')
                    {
                        if (beam.Direction == Coordinate.Right)
                        {
                            beam.Direction = Coordinate.Down;
                        }
                        else if (beam.Direction == Coordinate.Left)
                        {
                            beam.Direction = Coordinate.Up;
                        }
                        else if (beam.Direction == Coordinate.Up)
                        {
                            beam.Direction = Coordinate.Left;
                        }
                        else if (beam.Direction == Coordinate.Down)
                        {
                            beam.Direction = Coordinate.Right;
                        }
                    }
                    else if (tile == '/')
                    {
                        if (beam.Direction == Coordinate.Right)
                        {
                            beam.Direction = Coordinate.Up;
                        }
                        else if (beam.Direction == Coordinate.Left)
                        {
                            beam.Direction = Coordinate.Down;
                        }
                        else if (beam.Direction == Coordinate.Up)
                        {
                            beam.Direction = Coordinate.Right;
                        }
                        else if (beam.Direction == Coordinate.Down)
                        {
                            beam.Direction = Coordinate.Left;
                        }
                    }
                    else if (tile == '-')
                    {
                        if (beam.Direction == Coordinate.Up || beam.Direction == Coordinate.Down)
                        {
                            beam.Active = false;
                            beams.Add(new Beam(next, Coordinate.Left));
                            beams.Add(new Beam(next, Coordinate.Right));
                        }
                    }
                    else if (tile == '|')
                    {
                        if (beam.Direction == Coordinate.Left || beam.Direction == Coordinate.Right)
                        {
                            beam.Active = false;
                            beams.Add(new Beam(next, Coordinate.Up));
                            beams.Add(new Beam(next, Coordinate.Down));
                        }
                    }
                }
                else
                {
                    beam.Active = false;
                }
            }
        }

        var energizedCount = energized.Select(e => e.Coordinate).Distinct().Count();
        return energizedCount;
    }

    private class Beam(Coordinate location, Coordinate direction, bool active = true)
    {
        public Coordinate Location { get; set; } = location;
        public Coordinate Direction { get; set; } = direction;
        public bool Active { get; set; } = active;
    }

    public object SolveSecondPart(Input input)
    {
        var map = input.GetAsCoordinateMap();

        var maxEnergization = 0;

        for (int x = 0; x <= map.MaxX(); x++)
        {
            // from the top
            var direction = Coordinate.Down;
            var location = new Coordinate(x, -1);
            var beam = new Beam(location, direction);
            var energizedCount = CalculateEnergization([beam], map);
            
            maxEnergization = Math.Max(maxEnergization, energizedCount);
            
            // from the bottom
            direction = Coordinate.Up;
            location = new Coordinate(x, map.MaxY() + 1);
            beam = new Beam(location, direction);
            energizedCount = CalculateEnergization([beam], map);
            
            maxEnergization = Math.Max(maxEnergization, energizedCount);
        }

        for (int y = 0; y <= map.MaxY(); y++)
        {
            // from the left
            var direction = Coordinate.Right;
            var location = new Coordinate(-1, y);
            var beam = new Beam(location, direction);
            var energizedCount = CalculateEnergization([beam], map);
            
            maxEnergization = Math.Max(maxEnergization, energizedCount);
            
            // from the right
            direction = Coordinate.Left;
            location = new Coordinate(map.MaxX() + 1, y);
            beam = new Beam(location, direction);
            energizedCount = CalculateEnergization([beam], map);
            
            maxEnergization = Math.Max(maxEnergization, energizedCount);
        }

        return maxEnergization;
    }

}