using Microsoft.Xna.Framework;

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