using System.Collections.Generic;

namespace Units.Order
{
	/// <summary>
	/// Represents the queue or primitive orders in a <see cref="IUnidad"/>
	/// </summary>
	public class OrderQueue
	{
		// TODO: hacer un IList que maneje bien las inserciones por los costados.

		List<IPrimitiveOrder> queueSkill { get; }

		/// <summary>
		/// Enqueue an order
		/// </summary>
		/// <param name="ord">Order</param>
		public void Queue (IPrimitiveOrder ord)
		{
			queueSkill.Add (ord);
		}

		/// <summary>
		/// Stacks an order by inserting it in the index zero.
		/// </summary>
		/// <param name="ord">Order</param>
		public void Stack (IPrimitiveOrder ord)
		{
			queueSkill.Insert (0, ord);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="IUnidad"/> of instance is idle.
		/// </summary>
		/// <value><c>true</c> if this instance is idle; otherwise, <c>false</c>.</value>
		public bool IsIdle { get { return queueSkill.Count == 0; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.OrderQueue"/> class.
		/// </summary>
		public OrderQueue ()
		{
			queueSkill = new List<IPrimitiveOrder> ();
		}
	}
}