﻿using Units.Equipment;
using Units.Buffs;

namespace Items
{
	public enum EquipSlot
	{
		None,
		Head,
		Body,
		MainHand
	}

	public interface IEquipment : IItem
	{
		EquipSlot Slot { get; }

		EquipmentManager Owner { get; set; }
	}
}