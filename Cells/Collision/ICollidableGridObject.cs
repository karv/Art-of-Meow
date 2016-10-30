using System.Collections.Generic;
using Cells.CellObjects;

namespace Cells.Collision
{
	/// <summary>
	/// Represents an object that can collide with other
	/// </summary>
	public interface ICollidableGridObject : IGridObject
	{
		/// <summary>
		/// Gets the set of collition rules for this object
		/// </summary>
		/// <returns>The collision rules.</returns>
		IEnumerable<ICollisionRule> GetCollisionRules ();
	}
}