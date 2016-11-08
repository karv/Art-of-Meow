using System.Linq;

namespace Cells.Collision
{
	/// <summary>
	/// Maneja las colisiones de los objetos
	/// </summary>
	public class CollisionSystem
	{
		/// <summary>
		/// Determines if a cell object can fit into a given cell
		/// </summary>
		/// <param name="obj">Object to check if fit</param>
		/// <param name="cell">Cell where to fit</param>
		public bool CanFill (ICollidableGridObject obj, Cell cell)
		{
			foreach (var colObj in cell.Objects.OfType<ICollidableGridObject> ())
			{
				foreach (var rule in obj.GetCollisionRules ())
				{
					if (rule.CollisionWith (colObj))
						return false;
				}
				foreach (var rule in colObj.GetCollisionRules ())
				{
					if (rule.CollisionWith (obj))
						return false;
				}
			}
			return true;
		}
	}
}