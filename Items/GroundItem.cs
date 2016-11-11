using Cells;
using Cells.CellObjects;
using Items;
using Moggle;

namespace Items
{
	/// <summary>
	/// Representa un <see cref="IItem"/> en el suelo.
	/// </summary>
	public class GroundItem : GridObject
	{
		/// <summary>
		/// Gets the <see cref="IItem"/> this object refers
		/// </summary>
		/// <value>The item class.</value>
		public IItem ItemClass { get; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Items.GroundItem"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Items.GroundItem"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("Ground {0}@{1}", ItemClass, Location);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.GroundItem"/> class.
		/// </summary>
		/// <param name="type">Item type</param>
		/// <param name="grid">Grid.</param>
		public GroundItem (IItem type, LogicGrid grid, BibliotecaContenido content)
			: base (type.DefaultTextureName, grid, content)
		{
			ItemClass = type;
			Depth = Depths.Foreground;
			UseColor = ItemClass.DefaultColor;
		}
	}
}