using System;
using Microsoft.Xna.Framework;

namespace Helper
{
	public static class Geometry
	{
		public static float SquaredEucludeanDistance (Point p0, Point p1)
		{
			var dx = (p0.X - p1.X);
			var dy = (p0.Y - p1.Y);
			return (dx * dx + dy * dy);
		}
	}
}

