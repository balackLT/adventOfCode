using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Utilities.Map
{
    public enum TurnDirection
    {
        LEFT,
        RIGHT
    }
    
    public readonly struct Coordinate : IEquatable<Coordinate>, IFormattable
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

        public readonly int X;
        public readonly int Y;
        
        public Coordinate(string input, char separator = ',')
        {
            var split = input.Split(separator);
            
            X = int.Parse(split[0]);
            Y = int.Parse(split[1]);
        }
        
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
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
        
        public List<Coordinate> GetAdjacent()
        {
            var adjacent = new List<Coordinate>
            {
                this + North, 
                this + South, 
                this + West, 
                this + East,
                this + North + West, 
                this + North + East, 
                this + South + West, 
                this + South + East
            };
            
            return adjacent;
        }

        public int ManhattanDistance()
        {
            return ManhattanDistance(Zero);
        }
        
        public int ManhattanDistance(Coordinate other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
        
        public Vector2 ToVector()
        {
            return new(X, Y);
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

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinate other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string numberGroupSeparator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            stringBuilder.Append('<');
            stringBuilder.Append(X.ToString(format, formatProvider));
            stringBuilder.Append(numberGroupSeparator);
            stringBuilder.Append(' ');
            stringBuilder.Append(Y.ToString(format, formatProvider));
            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }
        
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append('<');
            stringBuilder.Append(X);
            stringBuilder.Append(',');
            stringBuilder.Append(' ');
            stringBuilder.Append(Y);
            stringBuilder.Append('>');
            return stringBuilder.ToString();
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

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }
    }
}