using System;
using System.Linq;
using AoM;
using Microsoft.Xna.Framework;
using Moggle.Comm;
using Moggle.Controles;
using MonoGame.Extended.InputListeners;

namespace Cells.CellObjects
{
	/// <summary>
	/// Escaleras como objeto de Grid
	/// </summary>
	public class StairsGridObject : GridObject
	{
		const string textureName = "stairs";

		/// <summary>
		/// </summary>
		/// <param name="grid">Grid.</param>
		public StairsGridObject (LogicGrid grid)
			: base (textureName, grid)
		{
			UseColor = Color.DarkOrange;
			Depth = Depths.Foreground;
		}
	}
}