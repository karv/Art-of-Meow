using Cells.CellObjects;
using Cells;

namespace Units
{
	public interface IUnidad : IGridObject
	{
		Grid MapGrid { get; }

		/// <summary>
		/// Try to damage a target.
		/// </summary>
		void MeleeDamage (IUnidad target);
	}

	static class UnidadImplementation
	{
		/// <summary>
		/// Muere esta unidad.
		/// </summary>
		public static void Die (this IUnidad u)
		{
			u.MapGrid.RemoveObject (u);
		}
	}
}