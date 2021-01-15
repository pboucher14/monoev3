using System;

namespace MonoBrickFirmware.Display
{
	public struct Point
	{
		#region Public Properties

		public int X { get; set; }
		public int Y { get; set; }

		#endregion

		#region Public Constructors

		public Point(int x, int y) : this()
		{
			this.X = x;
			this.Y = y;
		}

		#endregion

		#region Public Methods

		static public Point operator -(Point lhs, Point rhs)
		{
			return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		static public Point operator *(Point p, int factor)
		{
			return new Point(p.X * factor, p.Y * factor);
		}

		static public Point operator *(Point p, double factor)
		{
			return new Point((int)Math.Round(p.X * factor), (int)Math.Round(p.Y * factor));
		}

		static public Point operator +(Point lhs, Point rhs)
		{
			return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		#endregion
	}

	public struct Rectangle
	{
		#region Public Properties

		public Point P1 { get; set; }
		public Point P2 { get; set; }

		#endregion

		#region Public Constructors

		public Rectangle(Point p1, Point p2) : this()
		{
			this.P1 = p1;
			this.P2 = p2;
		}

		public Rectangle(int x1, int y1, int x2, int y2) : this()
		{
			this.P1 = new Point(x1, y1);
			this.P2 = new Point(x2, y2);
		}

		#endregion

		#region Public Methods

		static public Rectangle operator -(Rectangle r, Point p)
		{
			return new Rectangle(r.P1 - p, r.P2 - p);
		}

		static public Rectangle operator +(Rectangle r, Point p)
		{
			return new Rectangle(r.P1 + p, r.P2 + p);
		}

		#endregion
	}
}