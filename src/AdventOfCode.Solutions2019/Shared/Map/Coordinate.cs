using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Solutions2019.Shared.Map
{
    public struct Coordinate : IEquatable<Coordinate>, IFormattable
    {
        public int X;
        public int Y;
        
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int ManhattanDistance()
        {
            var other = new Coordinate(0, 0);
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
        
        public int ManhattanDistance(Coordinate other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }
        
        public Vector2 ToVector()
        {
            return new Vector2(X, Y);
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
            stringBuilder.Append(this.X.ToString(format, formatProvider));
            stringBuilder.Append(numberGroupSeparator);
            stringBuilder.Append(' ');
            stringBuilder.Append(this.Y.ToString(format, formatProvider));
            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }

        public static Coordinate operator +(Coordinate left, Coordinate right)
        {
            return new Coordinate(left.X + right.X, left.Y + right.Y);
        }
        
        public static Coordinate operator -(Coordinate left, Coordinate right)
        {
            return new Coordinate(left.X - right.X, left.Y - right.Y);
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