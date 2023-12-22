using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2023.Day22;

public class Solution : ISolution
{
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines().ToList();

        List<Brick> bricks = ParseBricks(lines);
        
        var map = new Dictionary<Coordinate3D, int>();
        foreach (var brick in bricks)
        {
            foreach (var coordinate in brick.GetCoordinates())
            {
                map[coordinate] = brick.Id;
            }
        }
        
        while (bricks.Any(b => b.Falling))
        {
            var brick = bricks.Where(b => b.Falling).OrderBy(b => b.LowestZ()).First();
            var brickLowestCoordinate = brick.GetLowestCoordinate();

            for (long z = brickLowestCoordinate.Z - 1; z >= 0; z--)
            {
                var brickLowCoordinates = brick.AllLowestCoordinates()
                    .Select(c => c with { Z = z }).ToList();
                foreach (Coordinate3D fallingSegment in brickLowCoordinates)
                {
                    if (map.TryGetValue(fallingSegment, out var value) && value != -1)
                    {
                        var newBrick = new Brick(brick.Id, 
                            brick.Start with { Z = z + 1 }, 
                            brick.End with { Z = z + 1 })
                        {
                            Falling = false
                        };

                        bricks.RemoveAll(b => b.Id == brick.Id);
                        bricks.Add(newBrick);

                        foreach (Coordinate3D oldCoordinate in brick.GetCoordinates())
                        {
                            map[oldCoordinate] = -1;
                        }

                        foreach (var newCoordinate in newBrick.GetCoordinates())
                        {
                            map[newCoordinate] = newBrick.Id;
                        }

                        z = -1;
                        break;
                    }
                }
                
                if (z == 0)
                {
                    var newBrick = new Brick(brick.Id, 
                        brick.Start with { Z = 0 }, 
                        brick.End with { Z = 0 })
                    {
                        Falling = false
                    };

                    bricks.RemoveAll(b => b.Id == brick.Id);
                    bricks.Add(newBrick);

                    foreach (Coordinate3D oldCoordinate in brick.GetCoordinates())
                    {
                        map[oldCoordinate] = -1;
                    }

                    foreach (var newCoordinate in newBrick.GetCoordinates())
                    {
                        map[newCoordinate] = newBrick.Id;
                    }

                    z = -1;
                    break;
                }
            }
        }

        foreach (Brick brick in bricks)
        {
            foreach (Coordinate3D segments in brick.AllLowestCoordinates())
            {
                var potentialLower = segments with { Z = segments.Z - 1 };
                if (map.TryGetValue(potentialLower, out var value) && value != -1)
                {
                    brick.SupportedBy.Add(value);
                }
            }
        }

        var safeToDestroy = 0;
        foreach (Brick brick in bricks)
        {
            var supportedBricks = bricks.Where(b => b.SupportedBy.Contains(brick.Id));
            if (supportedBricks.All(b => b.SupportedBy.Count > 1))
                safeToDestroy++;
        }
        
        return safeToDestroy;
    }

    private static List<Brick> ParseBricks(List<string> lines)
    {
        List<Brick> bricks = [];

        var i = 0;
        foreach (string line in lines)
        {
            var split = line.Split("~");
            var start = new Coordinate3D(split[0]);
            var end = new Coordinate3D(split[1]);

            var brick = new Brick(i, start, end);
            bricks.Add(brick);
            i++;
        }

        return bricks;
    }

    private record Brick(int Id, Coordinate3D Start, Coordinate3D End)
    {
        public bool Falling { get; set; } = true;
        public HashSet<int> SupportedBy = [];

        public IEnumerable<Coordinate3D> GetCoordinates()
        {
            return Start.CoordinatesStraightBetween(End);
        }
        
        public long LowestZ()
        {
            return Math.Min(Start.Z, End.Z);
        }

        public IEnumerable<Coordinate3D> AllLowestCoordinates()
        {
            var lowestZ = LowestZ();
            foreach (Coordinate3D coordinate3D in GetCoordinates())
            {
                if (coordinate3D.Z == lowestZ)
                    yield return coordinate3D;
            }
        }
        
        public Coordinate3D GetLowestCoordinate()
        {
            return new Coordinate3D(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y), Math.Min(Start.Z, End.Z));
        }
    }

    public object SolveSecondPart(Input input)
    {
        var lines = input.GetLines().ToList();

        return 0;
    }
}