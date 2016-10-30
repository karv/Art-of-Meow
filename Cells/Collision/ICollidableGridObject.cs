using System.Collections.Generic;

namespace Cells.Collision
{
	public interface ICollidableGridObject
	{
		IEnumerable<ICollisionRule> GetCollisionRules ();
	}
}