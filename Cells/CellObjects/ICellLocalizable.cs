using System;

namespace Cells.CellObjects
{
	[ObsoleteAttribute]
	public interface ICellLocalizable
	{
		/// <summary>
		/// Devuelve el objeto de celda asociado a esta unidad.
		/// </summary>
		/// <value>The cell object.</value>
		ICellObject CellObject { get; }
	}
	
}