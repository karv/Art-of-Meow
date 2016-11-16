using System;
using Microsoft.Xna.Framework;

namespace Helper
{
	/// <summary>
	/// Provee métodos de geometría
	/// </summary>
	public static class Geometry
	{
		/// <summary>
		/// Devuelve el cuadrado de la distancia Eucludiana entre dos <see cref="Point"/>
		/// </summary>
		public static float SquaredEucludeanDistance (Point p0, Point p1)
		{
			var dx = (p0.X - p1.X);
			var dy = (p0.Y - p1.Y);
			return (dx * dx + dy * dy);
		}
	}
}