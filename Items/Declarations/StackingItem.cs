using Items;

namespace Items.Declarations
{
	public abstract class StackingItem : CommonItemBase, IStackingItem
	{
		public int Quantity { get; set; }

		public override float Value { get { return base.Value * Quantity; } }

		public override string ToString ()
		{
			return string.Format ("{0}x{1}", NombreBase, Quantity);
		}

		protected StackingItem (string nombre, string [] allowedModNames)
			: base (nombre, allowedModNames)
		{
		}
	}
}