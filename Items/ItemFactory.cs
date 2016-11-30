using System;
using AoM;
using Items.Declarations.Equipment;
using Items.Declarations.Equipment.Skills;
using Items.Declarations.Pots;
using Microsoft.Xna.Framework;
using Moggle;

namespace Items
{
	/// <summary>
	/// Tipo de objeto
	/// </summary>
	public enum ItemType
	{
		/// <summary>
		/// Sword
		/// </summary>
		Sword,
		/// <summary>
		/// Un arco
		/// </summary>
		Bow,
		/// <summary>
		/// Potion
		/// </summary>
		/// <seealso cref="Items.Declarations.Pots.HealingPotion"/>
		HealingPotion,
		/// <summary>
		/// Armadura de cuero
		/// </summary>
		LeatherArmor,
		/// <summary>
		/// Casco de cuero
		/// </summary>
		LeatherCap
	}

	/// <summary>
	/// This class produces new items from its type
	/// </summary>
	public static class ItemFactory
	{
		static BibliotecaContenido contentManager { get { return Program.MyGame.Contenido; } }

		/// <summary>
		/// Creates a new item of the given type and casts it into a given type
		/// </summary>
		/// <returns>A newly created item</returns>
		/// <param name="type">Type of the item</param>
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
				case ItemType.Sword:
					ret = new MeleeWeapon ("Espada", "Items//katana", 1.4f, 0.7f){ Color = Color.DarkBlue };
					break;
				case ItemType.Bow:
					ret = new GenericSkillListEquipment (
						"Arco",
						new Units.Skills.ISkill[] { new RangedDamage { TextureName = "Items//bow_orange" } },
						EquipSlot.MainHand, 
						"Items//bow_orange"
					);
					break;
				case ItemType.HealingPotion:
					ret = new HealingPotion ();
					break;
				case ItemType.LeatherArmor:
					ret = new GenericArmor ("Leather Armor", EquipSlot.Body)
					{
						Color = Color.OrangeRed,
						TextureNameGeneric = "Items//body armor"
					};
					break;
				case ItemType.LeatherCap:
					ret = new GenericArmor ("Leather Cap", EquipSlot.Head)
					{
						Color = Color.OrangeRed,
						TextureNameGeneric = "Items//helmet"
					};
					break;
				default:
					throw new Exception ();
			}
			return ret;
		}
	}
}