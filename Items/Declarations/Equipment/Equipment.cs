using System.Diagnostics;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Abstract generic class for equipment
	/// </summary>
	public abstract class Equipment : CommonItemBase, IEquipment
	{
		/// <summary>
		/// Gets the slot where it can be equiped
		/// </summary>
		/// <value>The slot.</value>
		public abstract EquipSlot Slot { get; }

		/// <summary>
		/// Gets or sets the equipment manager where this equipment belongs
		/// </summary>
		public Units.Equipment.EquipmentManager Owner { get; set; }

		/// <summary>
		/// Unloads the content by unequiping this.
		/// </summary>
		protected override void UnloadContent ()
		{
			Debug.WriteLineIf (
				Owner == null,
				"Disposing equiped item " + this,
				"Equipment UnloadContent");
			Owner?.UnequipItem (this);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Items.Declarations.Equipment.Equipment"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Items.Declarations.Equipment.Equipment"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[Equipment: Slot={0}, Owner={1}, Nombre={2}]",
				Slot,
				Owner,
				Nombre);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.Equipment"/> class.
		/// </summary>
		/// <param name="nombre">Name of the equipment.</param>
		protected Equipment (string nombre)
			: base (nombre)
		{
		}
	}
}