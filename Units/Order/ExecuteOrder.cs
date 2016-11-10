using System;

namespace Units.Order
{
	/// <summary>
	/// This rder evecutes a <see cref="System.Action{IUnidad}"/>
	/// </summary>
	public class ExecuteOrder : PrimitiveOrder
	{
		/// <summary>
		/// Gets a value indicating whether this order can be cancelled.
		/// </summary>
		protected override bool CanCancel { get { return true; } }

		/// <summary>
		/// Gets a value indicating if this order is finished
		/// </summary>
		public override bool Finished { get { return true; } }

		/// <summary>
		/// This action will be invoked on it's turn
		/// </summary>
		public Action<IUnidad> ActionOnFinish { get; protected set; }

		/// <summary>
		/// Finish this instance.
		/// </summary>
		public override void Finish ()
		{
			// Invoke the action
			ActionOnFinish.Invoke (Unidad);
			base.Finish ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Order.ExecuteOrder"/> class.
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		/// <param name="actionOnFinish">Action on finish.</param>
		public ExecuteOrder (IUnidad unidad, Action<IUnidad> actionOnFinish)
			: base (unidad)
		{
			ActionOnFinish = actionOnFinish;
		}
	}

	/// <summary>
	/// This rder evecutes a <see cref="System.Action{IUnidad, T}"/>
	/// </summary>
	public class ExecuteOrder<T> : PrimitiveOrder
	{
		/// <summary>
		/// Gets or sets the context to pass to <see cref="ActionOnFinish"/>
		/// </summary>
		/// <value>The context.</value>
		public T Context { get; set; }

		/// <summary>
		/// Gets a value indicating whether this order can be cancelled.
		/// </summary>
		protected override bool CanCancel { get { return true; } }

		/// <summary>
		/// Gets a value indicating if this order is finished
		/// </summary>
		public override bool Finished { get { return true; } }

		/// <summary>
		/// This action will be invoked on it's turn
		/// </summary>
		public Action<IUnidad, T> ActionOnFinish { get; private set; }

		/// <summary>
		/// Finish this instance.
		/// </summary>
		public override void Finish ()
		{
			// Invoke the action
			ActionOnFinish.Invoke (Unidad, Context);
			base.Finish ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Order.ExecuteOrder{T}"/> class.
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		/// <param name="actionOnFinish">Action on finish.</param>
		public ExecuteOrder (IUnidad unidad, Action<IUnidad, T> actionOnFinish)
			: base (unidad)
		{
			ActionOnFinish = actionOnFinish;
		}
	}
}