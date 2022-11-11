using System;
using System.ComponentModel;

namespace RobotTools.UI.Editor
{
    public struct Location : IComparable<Location>, IEquatable<Location>
    {
        public static readonly Location Empty = new Location(-1, -1);
        private int _x;
        private int _y;


        public Location(int column, int line)
        {
            _x = column;
            _y = line;
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Line
        {
            get { return Y; }
            set { Y = value; }
        }

        public int Column
        {
            get { return X; }
            set { X = value; }
        }

        public bool IsEmpty
        {
            get { return X <= 0 && Y <= 0; }
        }

        public int CompareTo(Location other)
        {
            int result;
            if (this == other)
            {
                result = 0;
            }
            else
            {
                if (this < other)
                {
                    result = -1;
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }

        public bool Equals(Location other)
        {
            return this == other;
        }

        [Localizable(false)]
        public override string ToString()
        {
            return string.Format("(Line {1}, Col {0})", X, Y);
        }

        public override int GetHashCode()
        {
            return 87 * X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Location && (Location)obj == this;
        }

        public static bool operator ==(Location a, Location b)
        {
            return a.X == b._x && a.Y == b._y;
        }

        public static bool operator !=(Location a, Location b)
        {
            return a.X != b._x || a.Y != b._y;
        }

        public static bool operator <(Location a, Location b)
        {
            return a.Y < b._y || (a.Y == b._y && a.X < b._x);
        }

        public static bool operator >(Location a, Location b)
        {
            return a.Y > b._y || (a.Y == b._y && a.X > b._x);
        }

        public static bool operator <=(Location a, Location b)
        {
            return !(a > b);
        }

        public static bool operator >=(Location a, Location b)
        {
            return !(a < b);
        }
    }
}
