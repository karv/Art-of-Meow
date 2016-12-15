using System;
using Cells.CellObjects;
using Microsoft.Xna.Framework;

namespace Units
{
	/// <summary>
	/// This object can be relocated by the <see cref="Cells.LogicGrid"/>
	/// </summary>
	public interface IGridMoveable : IGridObject
	{
		/// <summary>
		/// Determines whether this instance can move to the specified destination.
		/// </summary>
		bool CanMove (Point destination);

		/// <summary>
		/// Executed before any movement
		/// </summary>
		void BeforeMoving (Point destination);

		/// <summary>
		/// Executes after any movement
		/// </summary>
		void AfterMoving (Point destination);

		/// <summary>
		/// Ocurrs when location changes
		/// </summary>
		event EventHandler OnRelocation;
	}
}