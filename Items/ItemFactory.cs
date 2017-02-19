using System;
using Items.Declarations.Equipment;
using Microsoft.Xna.Framework;

namespace Items
{
	/// <summary>
	/// This class produces new items from its type
	/// </summary>
	[Obsolete]
	public static class ItemFactory
	{
		/// <summary>
		/// Creates a new item of the given type and casts it into a given type
		/// </summary>
		/// <returns>A newly created item</returns>
		/// <param name="type">The way the item should be created</param>
		/// <exception cref="T:System.InvalidCastException">The return item is not of the given type</exception>
		public static T CreateItem<T> (ItemType type)
			where T : IItem
		{
			var item = CreateItem (type);
			if (!(item is T))
				throw new InvalidCastException ();
			var ret = (T)item;
			ret.Initialize ();
			return ret;
		}

		/// <summary>
		/// Creates a new item of the given type
		/// </summary>
		/// <returns>A newly created item</returns>
		/// <param name="type">Type of the item</param>
		public static IItem CreateItem (ItemType type)
		{
			IItem ret;
			switch (type)
			{
				case ItemType.Knife:
					ret = new MeleeWeapon ("Cuchillo", "Items//katana", 0.8f, 1.1f)
					{ Color = Color.DarkBlue, BaseSpeed = 1.8f };
					break;

				case ItemType.Sword:
					ret = new MeleeWeapon ("Espada", "Items//katana", 1.4f, 0.7f){ Color = Color.DarkBlue };
					break;

				case ItemType.Martillo:
					ret = new MeleeWeapon ("Cuchillo", "Items//katana", 1.6f, 0.5f)
					{ Color = Color.DarkBlue, BaseSpeed = 0.8f };
					break;

				case ItemType.Bow:
					return null;
/*					ret = new GenericSkillListEquipment (
						"Arco",
						new Units.Skills.ISkill[] { new RangedSkill { TextureName = "Items//bow_orange" } },
						EquipSlot.MainHand, 
						"Items//bow_orange"
					);
					break;
					*/

				case ItemType.LeatherArmor:
					ret = new GenericArmor ("Leather Armor", EquipSlot.Body)
					{
						Color = Color.OrangeRed
					};
					break;

				case ItemType.LeatherCap:
					ret = new GenericArmor ("Leather Cap", EquipSlot.Head)
					{
						Color = Color.OrangeRed,
					};
					break;

				default:
					throw new Exception ();
			}
			return ret;
		}
	}
}