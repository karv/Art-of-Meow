using System;
using Cells;
using Cells.CellObjects;
using Items;
using Units.Buffs;
using Units.Equipment;
using Units.Recursos;
using Units.Skills;

namespace Units
{
	/// <summary>
	/// Represents a unit in the game grid
	/// </summary>
	public interface IUnidad : IUpdateGridObject
	{
		/// <summary>
		/// Gets the map grid
		/// </summary>
		Grid MapGrid { get; }

		/// <summary>
		/// Try to damage a target.
		/// </summary>
		void MeleeDamage (IUnidad target);

		/// <summary>
		/// Gets the resources of this unit
		/// </summary>
		/// <value>The recursos.</value>
		ManejadorRecursos Recursos { get; }

		/// <summary>
		/// Gets the equipment of this unit
		/// </summary>
		/// <value>The equipment.</value>
		EquipmentManager Equipment { get; }

		/// <summary>
		/// Gets the buffs if this unit
		/// </summary>
		/// <value>The buffs.</value>
		BuffManager Buffs { get; }

		/// <summary>
		/// Gets the skills of this unit
		/// </summary>
		/// <value>The skills.</value>
		SkillManager Skills { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Units.IUnidad"/> is enabled, and therefore may act.
		/// </summary>
		bool Habilitado { get; }

		/// <summary>
		/// Gets the id of the team of this unit
		/// </summary>
		/// <value>The equipo.</value>
		int Equipo { get; }

		/// <summary>
		/// Gets the inventory of this unit
		/// </summary>
		IInventory Inventory { get; }
	}

	static class UnidadImplementation
	{
		/// <summary>
		/// Muere esta unidad.
		/// </summary>
		[Obsolete ("El encargado de esto es Grid.")]
		public static void Die (this IUnidad u)
		{
			u.MapGrid.RemoveObject (u);
		}
	}
}