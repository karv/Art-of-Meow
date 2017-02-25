using Newtonsoft.Json;
using Skills;

namespace Items.Declarations
{
	public class Arrow : StackingItem, IEquipment, IEffectAgent
	{
		public EquipSlot Slot
		{
			get { return EquipSlot.Quiver; }
		}

		public float DamageMultiplier = 1;
		public float BaseHit = 0.7f;

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