using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Cells.CellObjects
{
	public interface ICellObject : IDisposable
	{
		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		Point Location { get; set; }

		Texture2D Texture { get; }

		void LoadContent ();

		/// <summary>
		/// Determina si este objeto evita que otro objeto pueda ocupar esta misma celda.
		/// </summary>
		bool Collision (ICellObject collObj);

		/// <summary>
		/// Draws this object in a position (screen-wise)
		/// </summary>
		/// <param name="area">TopLeft of the output.</param>
		void Draw (Rectangle area, SpriteBatch bat);
	}
}