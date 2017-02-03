using System;
using Items.Declarations.Equipment;
using Items.Declarations.Equipment.Skills;
using Items.Declarations.Pots;
using Microsoft.Xna.Framework;

namespace Items
{
	/// <summary>
	/// Tipo de objeto
	/// </summary>
	public enum ItemType
	{
		/// <summary>
		/// Una daga corta de poco daño y buena puntería
		/// </summary>
		Knife,
		/// <summary>
		/// Sword
		/// </summary>
		Sword,
		/// <summary>
		/// Un martillo
		/// </summary>
		Martillo,
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

	public interface IItemFactory
	{
		IItem Create ();
	}

	public class SimpleItemRecipe : IItemFactory
	{
		public ItemType Type;

		public IItem Create ()
		{
			return ItemFactory.CreateItem (Type);
		}
	}

	public class RandomItemRecipe : IItemFactory
	{
		readonly static Random _r = new Random ();
		// TODO: ItemVal not impemented
		public float MinItemVal;
		public float MaxItemVal;
		public ItemType [] AllowedTypes;

		public IItem Create ()
		{
			var type = AllowedTypes [_r.Next (AllowedTypes.Length)];
			return ItemFactory.CreateItem (type);
		}

		public RandomItemRecipe ()
		{
			MinItemVal = 0;
			MaxItemVal = float.PositiveInfinity;
		}
	}

	/// <summary>
	/// This class produces new items from its type
	/// </summary>
	public static class ItemFactory
	{
		/// <summary>
		/// Creates a new item of the given type and casts it into a given type
		/// </summary>
		/// <returns>A newly created item</returns>
		/// <param name="recipe">The way the item should be created</param>
		/// <exception cref="T:System.InvalidCastException">The return item is not of the given type</exception>
		public static T CreateItem<T> (SimpleItemRecipe recipe)
			where T : IItem
		{
			var item = CreateItem (recipe.Type);
			if (!(item is T))
				throw new InvalidCastException ();
			var ret = (T)item;
			ret.Initialize ();
			return ret;
		}

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