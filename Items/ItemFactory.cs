using System;
using Items.Declarations.Equipment;
using Items.Declarations.Pots;


namespace Items
{
	public enum ItemType
	{
		Sword,
		Potion
	}

	public static class ItemFactory
	{
		public static IItem CreateItem (ItemType type)
		{
			switch (type)
			{
				case ItemType.Sword:
					return new Sword ();
				case ItemType.Potion:
					return new HealingPotion ();
				default:
					throw new Exception ();
			}
		}
	}
}

