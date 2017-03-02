using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Skills;

namespace Cells.CellObjects
{
	/// <summary>
	/// Represents a grid object that can be seen in the minimap
	/// </summary>
	public interface IMinimapVisible : IGridObject
	{
		/// <summary>
		/// Gets the color if the pixel in the minimap representing this object
		/// </summary>
		Color MinimapColor { get; }
	}
	
}