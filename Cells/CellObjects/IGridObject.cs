using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Content;

namespace Cells.CellObjects
{
	public interface IGridObject : IDisposable
	{
		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		Point Location { get; set; }

		Texture2D Texture { get; }

		void LoadContent (ContentManager content);

		/// <summary>
		/// Determina si este objeto evita que otro objeto pueda ocupar esta misma celda.
		/// </summary>
		bool Collision (IGridObject collObj);

		/// <summary>
		/// Draws this object in a position (screen-wise)
		/// </summary>
		/// <param name="area">TopLeft of the output.</param>
		/// <param name="bat">SpriteBatch de dibujo</param>
		void Draw (Rectangle area, SpriteBatch bat);
	}
}