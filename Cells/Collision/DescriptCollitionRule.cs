using Cells.Collision;
using System;

namespace Cells.Collision
{
	/// <summary>
	/// A collition rule build from a predicate
	/// </summary>
	public class DescriptCollitionRule : ICollisionRule
	{
		/// <summary>
		/// Gets the rule predicate
		/// </summary>
		/// <value>The collision with predicate.</value>
		public Predicate<ICollidableGridObject> CollisionWithPredicate { get; }

		bool ICollisionRule.CollisionWith (ICollidableGridObject other)
		{
			return CollisionWithPredicate (other);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.Collision.DescriptCollitionRule"/> class.
		/// </summary>
		/// <param name="func">Predicate that checks for collition</param>
		public DescriptCollitionRule (Predicate<ICollidableGridObject> func)
		{
			if (func == null)
				throw new ArgumentNullException ("func");
			CollisionWithPredicate = func;
		}
	}
}