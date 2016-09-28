using Cells.CellObjects;
using Items;
using Cells;

namespace Items
{
	/// <summary>
	/// Representa un <see cref="IItem"/> en el suelo.
	/// </summary>
	public class GroundItem : GridObject
	{
		public IItem ItemClass { get; }

		public GroundItem (IItem type, Grid grid)
			: base (type.DefaultTextureName, grid)
		{
			ItemClass = type;
		}
	}
}