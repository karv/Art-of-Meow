using Units.Equipment;
using System.Collections.Generic;
using Units.Skills;
using System.Collections;
using System;
using System.Diagnostics;
using System.Linq;
using Items.Modifiers;

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
		None,
		/// <summary>
		/// The head.
		/// </summary>
		Head,
		/// <summary>
		/// Body
		/// </summary>
		Body,
		/// <summary>
		/// The dominant hand
		/// </summary>
		MainHand
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

		ICollection <ItemModifier> Modifiers { get; }
		// THINK: Hacer una clase, en lugar de collection
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

	public static class EquipmentExt
	{
		public static string NombreMostrado (this IEquipment eq)
		{
			var ret = eq.Modifiers.Aggregate (
				          "",
				          (acc, iter) => agregarNombreModificado (
					          acc,
					          iter));

			return ret;
		}

		static string agregarNombreModificado (string nombreBase, ItemModifier mod)
		{
			switch (mod.NameUsage)
			{
				case ItemModifierNameUsage.Prefix:
					return string.Format ("{0} {1}", mod.Name + nombreBase);
				case ItemModifierNameUsage.Sufix:
					return string.Format ("{0} {1}", mod.Name + nombreBase);
			}
			throw new Exception (string.Format ("{0} not implemented.", mod.NameUsage));
		}
	}
}