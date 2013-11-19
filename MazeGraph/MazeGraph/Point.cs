using System;

namespace MazeGraph
{
	public struct Point
	{
		private int x;
		private int y;

		public int X { get { return x; } }
		public int Y { get { return y; } }

		public Point (int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is Point))
				return false;

			Point other = (Point)obj;
			return (this.X == other.X && this.Y == other.Y);
		}

		public static bool operator == (Point p1, Point p2)
		{
			return p1.Equals (p2);
		}

		public static bool operator != (Point p1, Point p2)
		{
			return !(p1.Equals (p2));
		}

		public static Point operator +(Point p1, Point p2)
		{
			return new Point (p1.x + p2.x, p1.y + p2.y);
		}

		public override int GetHashCode ()
		{
			return (x + y).GetHashCode ();
		}

		public override string ToString ()
		{
			return string.Format ("({0}, {1})", X, Y);
		}
	}
}

