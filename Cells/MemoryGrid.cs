using System;
using Units;

namespace Cells
{
	/// <summary>
	/// Representa los <c>recuerdos</c> de un tablero de una unidad
	/// </summary>
	public class MemoryGrid
	{
		/// <summary>
		/// Devuelve el tablero que se está memorizando
		/// </summary>
		public LogicGrid MemorizingGrid { get; }

		/// <summary>
		/// Devuelve la unidad que posee <c>la memoria</c>
		/// </summary>
		public IUnidad Unidad { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.MemoryGrid"/> class.
		/// </summary>
		/// <param name="grid">Tablero memorizando</param>
		/// <param name="unid">Unidad de memoria</param>
		public MemoryGrid (LogicGrid grid, IUnidad unid)
		{
			if (grid == null)
				throw new ArgumentNullException ("grid");
			if (unid == null)
				throw new ArgumentNullException ("unid");
			
			MemorizingGrid = grid;
			Unidad = unid;
		}
	}
}