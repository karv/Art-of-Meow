using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Cells.CellObjects
{
	public interface ICellLocalizable
	{
		/// <summary>
		/// Devuelve el objeto de celda asociado a esta unidad.
		/// </summary>
		/// <value>The cell object.</value>
		ICellObject CellObject { get; }
	}
	
}