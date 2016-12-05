using System;
using Cells;
using Microsoft.Xna.Framework;

namespace AoM
{
	/// <summary>
	/// Representa un punto en el mundo;
	/// Es una pareja ordenada <see cref="LogicGrid"/> y <see cref="Point"/>
	/// </summary>
	public struct WorldLocation : IEquatable<WorldLocation>
	{
		/// <summary>
		/// El tablero
		/// </summary>
		public LogicGrid Grid { get; }

		/// <summary>
		/// Punto en el tablero
		/// </summary>
		/// <value>The grid point.</value>
		public Point GridPoint { get; }

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="AoM.WorldLocation"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="AoM.WorldLocation"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="AoM.WorldLocation"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			if (obj is WorldLocation)
				return Equals ((WorldLocation)obj);
			return false;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="AoM.WorldLocation"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			unchecked
			{
				return (Grid.GetHashCode ()) ^ GridPoint.GetHashCode ();
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="AoM.WorldLocation"/> is equal to the current <see cref="AoM.WorldLocation"/>.
		/// </summary>
		/// <param name="other">The <see cref="AoM.WorldLocation"/> to compare with the current <see cref="AoM.WorldLocation"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="AoM.WorldLocation"/> is equal to the current
		/// <see cref="AoM.WorldLocation"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (WorldLocation other)
		{
			return ReferenceEquals (Grid, other.Grid) && GridPoint == other.GridPoint;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AoM.WorldLocation"/> struct.
		/// </summary>
		/// <param name="grid">Grid.</param>
		/// <param name="point">Point.</param>
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