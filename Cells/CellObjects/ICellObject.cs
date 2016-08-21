using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Cells.CellObjects
{

	public interface ICellObject
	{
		Texture2D Texture { get; }

		int Depth { get; }

		Color? UseColor { get; }

		void LoadContent ();
	}
}