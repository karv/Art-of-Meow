using Items;

namespace Items.Declarations
{
	/// <summary>
	/// The generic implementation of <see cref="IStackingItem"/>
	/// </summary>
	public abstract class StackingItem : CommonItemBase, IStackingItem
	{
		/// <summary>
		/// Quantity of stacked items
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// Gets the value or worth of the item
		/// </summary>
		public override float Value { get { return base.Value * Quantity; } }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Items.Declarations.StackingItem"/>.
		/// </summary>
		public override string ToString ()
		{
			return string.Format ("{0}x{1}", NombreBase, Quantity);
		}

		/// <summary></summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name="allowedModNames">Allowed mod names.</param>
		protected StackingItem (string nombre, string [] allowedModNames)
			: base (nombre, allowedModNames)
		{
		}
	}
}