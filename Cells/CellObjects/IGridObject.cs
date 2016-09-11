using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using Componentes;

namespace Cells.CellObjects
{
	public interface IGridObject : IGameComponent, IDisposable, IRelDraw
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

	}
}