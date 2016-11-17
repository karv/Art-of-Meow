using System;
using AoM;
using Items.Declarations.Equipment;
using Items.Declarations.Pots;
using Microsoft.Xna.Framework;
using Moggle;
using Items.Declarations.Equipment.Skills;

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
		/// <seealso cref="Items.Declarations.Equipment.Sword"/>
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
					ret = new Sword ();
					break;
				case ItemType.Bow:
					ret = new GenericSkillListEquipment (
						"Arco",
						// TODO: Buscar icono
						new Units.Skills.ISkill[] { new RangedDamage { TextureName = "pixel" } },
						EquipSlot.MainHand, 
						"pixel"
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
			ret.Initialize ();
			return ret;
		}
	}
}