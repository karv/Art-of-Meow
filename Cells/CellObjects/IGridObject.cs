using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;

namespace Cells.CellObjects
{
	/// <summary>
	/// Representa un objeto como miembro de un <see cref="Grid"/>
	/// </summary>
	public interface IGridObject : IControl, IDisposable, IDibujable
	{
		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		Point Location { get; set; }

		/// <summary>
		/// Gets the texture used to draw this object
		/// </summary>
		/// <value>The texture.</value>
		Texture2D Texture { get; }

		/// <summary>
		/// Gets the containing <see cref="Grid"/>
		/// </summary>
		/// <value>The grid.</value>
		Grid Grid { get; }
	}
}