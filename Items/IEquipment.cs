﻿using System.Collections.Generic;
using Units.Equipment;
using Skills;

namespace Items
{
	/// <summary>
	/// Represents a slot where a <see cref="IEquipment"/> may be equiped
	/// </summary>
	public enum EquipSlot
	{
		/// <summary>
		/// Cannot be equiped
		/// </summary>
		None = 0,
		/// <summary>
		/// The head.
		/// </summary>
		Head = 1,
		/// <summary>
		/// Body
		/// </summary>
		Body = 2,
		/// <summary>
		/// The dominant hand
		/// </summary>
		MainHand = 3,
		/// <summary>
		/// Arrow, etc
		/// </summary>
		Quiver = 4,
	}

	/// <summary>
	/// Represents an item that can be equiped
	/// </summary>
	public interface IEquipment : IItem
	{
		/// <summary>
		/// Gets the slot where it can be equiped
		/// </summary>
		EquipSlot Slot { get; }

		/// <summary>
		/// Gets or sets the maanger of the currenty equiped <see cref="Units.IUnidad"/>
		/// </summary>
		EquipmentManager Owner { get; set; }
	}

	/// <summary>
	/// Un equipamento que ofrece Skills al usuario
	/// </summary>
	public interface ISkillEquipment : IEquipment
	{
		/// <summary>
		/// Devuelve la lista de skills que ofrece al usuario
		/// </summary>
		IEnumerable<ISkill> GetEquipmentSkills ();
	}
}