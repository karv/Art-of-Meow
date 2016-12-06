using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Helper
{
	/// <summary>
	/// Provee métodos de geometría
	/// </summary>
	public static class Geometry
	{
		/// <summary>
		/// Enumera los puntos de un rectángulo
		/// </summary>
		public static IEnumerable<Point> EnumeratePoints (this Rectangle rect)
		{
			for (int ix = rect.Left; ix <= rect.Right; ix++)
				for (int iy = rect.Top; iy <= rect.Bottom; iy++)
					yield return new Point (ix, iy);
		}

		/// <summary>
		/// Devuelve el cuadrado de la distancia Eucludiana entre dos <see cref="Point"/>
		/// </summary>
		public static float SquaredEucludeanDistance (Point p0, Point p1)
		{
			var dx = (p0.X - p1.X);
			var dy = (p0.Y - p1.Y);
			return (dx * dx + dy * dy);
		}

		/// <summary>
		/// Enumera las coordenedas de los puntos que están en el segmento que une dos puntos dados
		/// </summary>
		/// <returns>Una sucesión de puntos</returns>
		/// <param name="origin">Punto inicial</param>
		/// <param name="final">Punto final</param>
		/// <param name="includeFinalPoint">Si debe incluir punto inicial</param>
		/// <param name="includeInitialPoint">Si debe incluir punto final</param>
		public static IEnumerable<Point> EnumerateLine (Point origin,
		                                                Point final,
		                                                bool includeInitialPoint = false,
		                                                bool includeFinalPoint = false)
		{
			if (includeInitialPoint)
				yield return origin;
			while (origin != final)
			{
				var dir = (final - origin).ToVector2 ();
				dir.Normalize ();
				const double sqrt_2 = 0.707;
				if (dir.Y > sqrt_2)
					origin = new Point (origin.X, origin.Y + 1);
				else if (dir.Y < -sqrt_2)
					origin = new Point (origin.X, origin.Y - 1);
				if (dir.X > sqrt_2)
					origin = new Point (origin.X + 1, origin.Y);
				else if (dir.X < -sqrt_2)
					origin = new Point (origin.X - 1, origin.Y);

				if (origin != final)
					yield return origin;
				else
				{
					if (includeFinalPoint)
						yield return final;
					yield break;
				}
			}
		}
	}
}