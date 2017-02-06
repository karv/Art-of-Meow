using System;

namespace Items
{
	/// <summary>
	/// Tipo de objeto
	/// </summary>
	[Obsolete]
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
}