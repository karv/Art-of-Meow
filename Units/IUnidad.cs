using Cells.CellObjects;
using Cells;
using Units.Recursos;
using MonoGame.Extended;
using Art_of_Meow;
using System;

namespace Units
{
	public interface IUnidad : IGridObject, IUpdate
	{
		Grid MapGrid { get; }

		/// <summary>
		/// Try to damage a target.
		/// </summary>
		void MeleeDamage (IUnidad target);

		ManejadorRecursos Recursos { get; }

		bool Habilitado { get; }
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