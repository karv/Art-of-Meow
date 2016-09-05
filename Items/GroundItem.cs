using Cells.CellObjects;
using Items;

namespace Items
{
	/// <summary>
	/// Representa un <see cref="IItem"/> en el suelo.
	/// </summary>
	public class GroundItem : GridObject
	{
		public IItem ItemClass { get; }

		public GroundItem (IItem type)
			: base (type.DefaultTextureName)
		{
			ItemClass = type;
		}
	}
}