using Cells;
using Microsoft.Xna.Framework;
using System;


namespace AoM
{
	public struct WorldLocation : IEquatable<WorldLocation>
	{
		public LogicGrid Grid { get; }

		public Point GridPoint { get; }

		public override bool Equals (object obj)
		{
			if (obj is WorldLocation)
				return Equals ((WorldLocation)obj);
			return false;
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				return (Grid.GetHashCode ()) ^ GridPoint.GetHashCode ();
			}
		}

		public bool Equals (WorldLocation other)
		{
			return ReferenceEquals (Grid, other.Grid) && GridPoint == other.GridPoint;
		}

		public WorldLocation (LogicGrid grid, Point point)
		{
			if (grid == null)
				throw new ArgumentNullException ("grid");

			if (point.X < 0 || point.Y < 0 ||
			    grid.Size.Width <= point.X || grid.Size.Height <= point.Y)
				throw new Exception ("Location is not instide the grid.");
			
			Grid = grid;
			GridPoint = point;
		}
	}
}