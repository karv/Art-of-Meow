using Newtonsoft.Json;

namespace Items.Declarations
{
	public class Arrow : StackingItem, IEquipment
	{
		public EquipSlot Slot
		{
			get { return EquipSlot.Quiver; }
		}

		public Units.Equipment.EquipmentManager Owner { get; set; }

		public override object Clone ()
		{
			return new Arrow (NombreBase, AllowedModNames)
			{ Quantity = Quantity, Attribute = Attribute };
		}

		public string Attribute;

		[JsonConstructor]
		Arrow (string nombre, string [] allowedModNames)
			: base (nombre, allowedModNames)
		{
		}
	}
}