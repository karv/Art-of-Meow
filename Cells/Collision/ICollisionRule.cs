
namespace Cells.Collision
{
	public interface ICollisionRule
	{
		bool CollisionWith (ICollidableGridObject other);
	}
}