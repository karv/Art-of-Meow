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
		/// Gets the current primitive order. Will throw <see cref="System.Exception"/> if <see cref="IsIdle"/>
		/// </summary>
		/// <value>The current order.</value>
		public IPrimitiveOrder CurrentOrder { get { return queueSkill [0]; } }

		bool isCurrentOrderStarted;

		/// <summary>
		/// Gets a value indicating whether the <see cref="IUnidad"/> of instance is idle.
		/// </summary>
		/// <value><c>true</c> if this instance is idle; otherwise, <c>false</c>.</value>
		public bool IsIdle { get { return queueSkill.Count == 0; } }

		/// <summary>
		/// Determines whether all the queue can be cancelled
		/// </summary>
		public bool CanCancel ()
		{
			return queueSkill.TrueForAll (z => z.CanCancel);
		}

		/// <summary>
		/// Updates the queue
		/// </summary>
		/// <param name="time">Game-time passed</param>
		public void PassTime (float time)
		{
			// TODO: por ahora sólo revisa un PrimitiveOrder por update.

			if (IsIdle)
				return;

			if (!isCurrentOrderStarted)
			{
				CurrentOrder.Start ();
				isCurrentOrderStarted = true;
			}
			CurrentOrder.PassTime (time);
			if (CurrentOrder.Finished)
			{
				CurrentOrder.Finish ();
				queueSkill.RemoveAt (0);
				isCurrentOrderStarted = false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Order.OrderQueue"/> class.
		/// </summary>
		public OrderQueue ()
		{
			queueSkill = new List<IPrimitiveOrder> ();
			isCurrentOrderStarted = false;
		}
	}
}