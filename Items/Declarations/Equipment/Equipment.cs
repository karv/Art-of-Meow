using System.Diagnostics;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Abstract generic class for equipment
	/// </summary>
	public abstract class Equipment : CommonItemBase, IEquipment
	{
		public abstract EquipSlot Slot { get; }

		public Units.Equipment.EquipmentManager Owner { get; set; }

		protected override void UnloadContent ()
		{
			Debug.WriteLineIf (
				Owner == null,
				"Disposing equiped item " + this,
				"Equipment UnloadContent");
			Owner?.UnequipItem (this);
		}

		public override string ToString ()
		{
			return string.Format (
				"[Equipment: Slot={0}, Owner={1}, Nombre={2}]",
				Slot,
				Owner,
				Nombre);
		}

		protected Equipment (string nombre)
			: base (nombre)
		{
		}
	}
}