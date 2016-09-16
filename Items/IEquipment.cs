using Units.Equipment;

namespace Items
{
	public enum EquipSlot
	{
		None,
		Head,
		Body
	}

	public interface IEquipment : IItem
	{
		EquipSlot Slot { get; }

		EquipmentManager Owner { get; set; }
	}
}