using System;
using Componentes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;

namespace Cells.CellObjects
{
	public interface IGridObject : IControl, IDisposable, IRelDraw
	{
		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		Point Location { get; set; }

		Texture2D Texture { get; }

		Grid Grid { get; }

		/// <summary>
		/// Determina si este objeto evita que otro objeto pueda ocupar esta misma celda.
		/// </summary>
		bool Collision (IGridObject collObj);
	}
}