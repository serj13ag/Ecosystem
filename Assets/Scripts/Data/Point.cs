using System;

namespace Data
{
    public readonly struct Point : IEquatable<Point>
    {
        public static Point Left { get; } = new Point(-1, 0);
        public static Point Right { get; } = new Point(1, 0);
        public static Point Up { get; } = new Point(0, 1);
        public static Point Down { get; } = new Point(0, -1);

        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point other)
            {
                return other.X == X && other.Y == Y;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(Point obj1, Point obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Point obj1, Point obj2)
        {
            return !(obj1 == obj2);
        }

        public static Point operator +(Point obj1, Point obj2)
        {
            return new Point(obj1.X + obj2.X, obj1.Y + obj2.Y);
        }

        public static Point operator -(Point obj1, Point obj2)
        {
            return new Point(obj1.X - obj2.X, obj1.Y - obj2.Y);
        }

        public static Point operator *(Point obj1, int obj2)
        {
            return new Point(obj1.X * obj2, obj1.Y * obj2);
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public static float Distance(Point a, Point b)
        {
            float num1 = a.X - b.X;
            float num2 = a.Y - b.Y;

            return (float)Math.Sqrt(num1 * (double)num1 + num2 * (double)num2);
        }
    }
}