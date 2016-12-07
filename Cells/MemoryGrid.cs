using System;
using Microsoft.Xna.Framework;
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

		readonly MemorizedCell [,] cellData;

		int sizeX { get { return cellData.GetLength (0); } }

		int sizeY { get { return cellData.GetLength (1); } }

		/// <summary>
		/// Devuelve la celda emorizada en un punto del tablero
		/// </summary>
		/// <param name="p">P.</param>
		public MemorizedCell this [Point p]
		{
			get
			{ 
				if (p.X < 0 || p.Y < 0 || p.X >= sizeX || p.Y >= sizeY)
					return MemorizedCell.VoidCell;
				return cellData [p.X, p.Y]; 
			}
			private set{ cellData [p.X, p.Y] = value; }
		}

		/// <summary>
		/// Devuelve la unidad que posee <c>la memoria</c>
		/// </summary>
		public IUnidad Unidad { get; }

		void storeCellInfo (Cell cell)
		{
			this [cell.Location] = cell.GetMemorizationClone ();
		}

		/// <summary>
		/// Actualiza la memoria de acuerdo a lo que la unidad 've'
		/// </summary>
		public void UpdateMemory ()
		{
			var vPoints = Unidad.VisiblePoints ();
			foreach (var p in vPoints)
				storeCellInfo (MemorizingGrid [p]);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.MemoryGrid"/> class.
		/// </summary>
		/// <param name="unid">Unidad de memoria</param>
		public MemoryGrid (IUnidad unid)
			: this (unid.Grid, unid)
		{
		}

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
			cellData = new MemorizedCell[grid.Size.Width, grid.Size.Height];
		}
	}
}