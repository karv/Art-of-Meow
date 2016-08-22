using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Cells.CellObjects
{
	public interface ICellObject
	{
		Texture2D Texture { get; }

		float Depth { get; }

		Color? UseColor { get; }

		void LoadContent ();

		/// <summary>
		/// Determina si este objeto evita que otro objeto pueda ocupar esta misma celda.
		/// </summary>
		bool Collision (ICellObject collObj);
	}
}