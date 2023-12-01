using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities.Map;

public record BigCoordinate(long X, long Y)
{
    public static readonly BigCoordinate Zero = new (0, 0);
    public static readonly BigCoordinate North = new (0, 1);
    public static readonly BigCoordinate South = new (0, -1);
    public static readonly BigCoordinate East = new (1, 0);
    public static readonly BigCoordinate West = new (-1, 0);
        
    public static readonly BigCoordinate NorthWest = new (-1, 1);
    public static readonly BigCoordinate NorthEast = new (1, 1);
    public static readonly BigCoordinate SouthWest = new (-1, -1);
    public static readonly BigCoordinate SouthEast = new (1, -1);
        
    public static readonly List<BigCoordinate> Directions = new() {North, South, West, East};
    public static readonly List<BigCoordinate> ExtendedDirections = new() {North, South, West, East, NorthEast, NorthWest, SouthEast, SouthWest};

    public BigCoordinate(string input, char separator = ',') : this(
        long.Parse(input.Split(separator)[0]),
        long.Parse(input.Split(separator)[1]))
    {
            
    }

    public static BigCoordinate Turn(BigCoordinate facing, TurnDirection turn)
    {
        return facing.Turn(turn);
    }

    public BigCoordinate Turn(TurnDirection turn)
    {
        return turn switch
        {
            TurnDirection.LEFT => new BigCoordinate(Y * -1, X),
            TurnDirection.RIGHT => new BigCoordinate(Y, X * -1),
            _ => throw new Exception("Invalid direction encountered")
        };
    }

    public IEnumerable<BigCoordinate> BigCoordinatesStraightBetween(BigCoordinate target)
    {
        var current = this;
        while (current != target)
        {
            yield return current;
            current = current.MoveTowards(target);
        }
        yield return current;
    }

    public bool IsAdjacentWithDiagonals(BigCoordinate coordinate)
    {
        var adjacent = GetAdjacentWithDiagonals();
        return adjacent.Contains(coordinate);
    }
    
    public BigCoordinate MoveTowardsWithDiagonals(BigCoordinate coordinate)
    {
        var adjacent = GetAdjacentWithDiagonals();
        return adjacent.MinBy(coordinate.ManhattanDistance);
    }
    
    public BigCoordinate MoveTowards(BigCoordinate coordinate)
    {
        var adjacent = GetAdjacentWithDiagonals();
        return adjacent.MinBy(coordinate.ManhattanDistance);
    }
    
    public IEnumerable<BigCoordinate> GetAdjacentWithDiagonals()
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
        
    public IEnumerable<BigCoordinate> GetAdjacent()
    {
        yield return this + North;
        yield return this + South;
        yield return this + West;
        yield return this + East;
    }

    public long ManhattanDistance()
    {
        return ManhattanDistance(Zero);
    }
        
    public long ManhattanDistance(BigCoordinate other)
    {
        return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }

    public static double AngleBetween(BigCoordinate source, BigCoordinate target)
    {
        var radians = Math.Atan2(target.Y - source.Y, target.X - source.X);

        return (180 / Math.PI) * radians;
    }

    public BigCoordinate Normalize()
    {
        if (X > 0) return new BigCoordinate(1, 0);
        if (X < 0) return new BigCoordinate(-1, 0);
        if (Y > 0) return new BigCoordinate(0, 1);
        if (Y < 0) return new BigCoordinate(0, -1);
            
        return new BigCoordinate(0, 0);
    }

    public static BigCoordinate operator +(BigCoordinate left, BigCoordinate right)
    {
        return new(left.X + right.X, left.Y + right.Y);
    }
        
    public static BigCoordinate operator -(BigCoordinate left, BigCoordinate right)
    {
        return new(left.X - right.X, left.Y - right.Y);
    }
        
    public static BigCoordinate operator *(BigCoordinate left, long right)
    {
        return new(left.X * right, left.Y * right);
    }
        
    public static BigCoordinate operator *(long left, BigCoordinate right)
    {
        return new(left * right.X, left * right.Y);
    }
}