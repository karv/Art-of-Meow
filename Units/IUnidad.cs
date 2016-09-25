using System;
using Cells;
using Cells.CellObjects;
using Items;
using Units.Buffs;
using Units.Equipment;
using Units.Recursos;

namespace Units
{
	public interface IUnidad : IUpdateGridObject, IExpGiver
	{
		Grid MapGrid { get; }

		/// <summary>
		/// Try to damage a target.
		/// </summary>
		void MeleeDamage (IUnidad target);

		ManejadorRecursos Recursos { get; }

		EquipmentManager Equipment { get; }

		ExpManager Exp { get; }

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