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
			IItem ret;
			switch (type)
			{
				case ItemType.Sword:
					ret = new Sword ();
					break;
				case ItemType.Potion:
					ret = new HealingPotion ();
					break;
				default:
					throw new Exception ();
			}
			ret.Initialize ();
			return ret;
		}
	}
}