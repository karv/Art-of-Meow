using System;
using Items.Declarations.Equipment;
using Items.Declarations.Pots;


namespace Items
{
	/// <summary>
	/// Gets the type of an item
	/// </summary>
	public enum ItemType
	{
		/// <summary>
		/// Sword
		/// </summary>
		/// <seealso cref="Items.Declarations.Equipment.Sword"/>
		Sword,
		/// <summary>
		/// Temporal, DELETE|REMOVE|ETC
		/// </summary>
		HealingSword,
		/// <summary>
		/// Potion
		/// </summary>
		/// <seealso cref="Items.Declarations.Pots.HealingPotion"/>
		Potion
	}

	/// <summary>
	/// This class produces new items from its type
	/// </summary>
	public static class ItemFactory
	{
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
				case ItemType.Potion:
					ret = new HealingPotion ();
					break;
				case ItemType.HealingSword:
					ret = new HealingSword ();
					break;
				default:
					throw new Exception ();
			}
			ret.Initialize ();
			return ret;
		}
	}
}