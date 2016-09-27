using System;
using Items.Declarations.Equipment;
using Items.Declarations.Pots;
using Microsoft.Xna.Framework;


namespace Items
{
	public enum ItemType
	{
		Sword,
		Potion,
		Leather_Armor,
		Leather_Cap
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
				case ItemType.Leather_Armor:
					ret = new GenericArmor ("Leather Armor", EquipSlot.Body)
					{
						Color = Color.OrangeRed,
						TextureNameGeneric = "" // TODO
					};
					break;
				case ItemType.Leather_Cap:
					ret = new GenericArmor ("Leather Cap", EquipSlot.Head)
					{
						Color = Color.OrangeRed,
						TextureNameGeneric = "" // TODO
					};
					break;
				default:
					throw new Exception ();
			}
			ret.Initialize ();
			return ret;
		}
	}
}