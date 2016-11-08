using Cells.Collision;

namespace Cells.Collision
{
	/// <summary>
	/// Represents a rule of collition
	/// </summary>
	public interface ICollisionRule
	{
		/// <summary>
		/// Determines this collides with some other object
		/// </summary>
		/// <param name="other">Some other collidable object</param>
		bool CollisionWith (ICollidableGridObject other);
	}
}