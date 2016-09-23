using System;
using Cells;
using Cells.CellObjects;
using Units.Recursos;
using Units.Equipment;
using Units.Buffs;
using Items;

namespace Units
{
	public interface IUnidad : IUpdateGridObject
	{
		Grid MapGrid { get; }

		/// <summary>
		/// Try to damage a target.
		/// </summary>
		void MeleeDamage (IUnidad target);

		ManejadorRecursos Recursos { get; }

		EquipmentManager Equipment { get; }

		BuffManager Buffs { get; }

		bool Habilitado { get; }

		int Equipo { get; }

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