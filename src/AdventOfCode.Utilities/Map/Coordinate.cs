using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities.Map;

public enum TurnDirection
{
    LEFT,
    RIGHT
}
    
public record Coordinate(int X, int Y)
{
    public static readonly Coordinate Zero = new (0, 0);
    public static readonly Coordinate North = new (0, 1);
    public static readonly Coordinate South = new (0, -1);
    public static readonly Coordinate East = new (1, 0);
    public static readonly Coordinate West = new (-1, 0);
        
    public static readonly Coordinate NorthWest = new (-1, 1);
    public static readonly Coordinate NorthEast = new (1, 1);
    public static readonly Coordinate SouthWest = new (-1, -1);
    public static readonly Coordinate SouthEast = new (1, -1);
        
    public static readonly List<Coordinate> Directions = new() {North, South, West, East};
    public static readonly List<Coordinate> ExtendedDirections = new() {North, South, West, East, NorthEast, NorthWest, SouthEast, SouthWest};

    public Coordinate(string input, char separator = ',') : this(
        int.Parse(input.Split(separator)[0]),
        int.Parse(input.Split(separator)[1]))
    {
            
    }

    public static Coordinate Turn(Coordinate facing, TurnDirection turn)
    {
        return facing.Turn(turn);
    }

    public Coordinate Turn(TurnDirection turn)
    {
        return turn switch
        {
            TurnDirection.LEFT => new Coordinate(Y * -1, X),
            TurnDirection.RIGHT => new Coordinate(Y, X * -1),
            _ => throw new Exception("Invalid direction encountered")
        };
    }

    public bool IsAdjacentWithDiagonals(Coordinate coordinate)
    {
        var adjacent = GetAdjacentWithDiagonals();
        return adjacent.Contains(coordinate);
    }
    
    public IEnumerable<Coordinate> GetAdjacentWithDiagonals()
    {
        yield return this + North;
        yield return this + South;
        yield return this + West;
        yield return this + East;
        yield return this + North + West;
        yield return this + North + East;
        yield return this + South + West;
        yield return this + South + East;
    }
        
    public IEnumerable<Coordinate> GetAdjacent()
    {
        yield return this + North;
        yield return this + South;
        yield return this + West;
        yield return this + East;
    }

    public int ManhattanDistance()
    {
        return ManhattanDistance(Zero);
    }
        
    public int ManhattanDistance(Coordinate other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public static double AngleBetween(Coordinate source, Coordinate target)
    {
        var radians = Math.Atan2(target.Y - source.Y, target.X - source.X);

        return (180 / Math.PI) * radians;
    }

    public Coordinate Normalize()
    {
        if (X > 0) return new Coordinate(1, 0);
        if (X < 0) return new Coordinate(-1, 0);
        if (Y > 0) return new Coordinate(0, 1);
        if (Y < 0) return new Coordinate(0, -1);
            
        return new Coordinate(0, 0);
    }

    public static Coordinate operator +(Coordinate left, Coordinate right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
        
    public static Coordinate operator -(Coordinate left, Coordinate right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }
        
    public static Coordinate operator *(Coordinate left, int right)
    {
        return new(left.X * right, left.Y * right);
    }
        
    public static Coordinate operator *(int left, Coordinate right)
    {
        return new(left * right.X, left * right.Y);
    }
}