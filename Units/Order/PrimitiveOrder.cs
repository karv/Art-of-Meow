using System;

namespace Units.Order
{
	/// <summary>
	/// Common implementation of IPrimitiveOrder
	/// </summary>
	public abstract class PrimitiveOrder : IPrimitiveOrder
	{
		/// <summary>
		/// Gets the unidad linked to this order
		/// </summary>
		public IUnidad Unidad { get; }

		/// <summary>
		/// Executed with the game update
		/// </summary>
		/// <param name="gameTime">Time passed</param>
		public virtual float PassTime (float gameTime)
		{
			return gameTime;
		}

		/// <summary>
		/// This method is executed when this order becomes first in the <see cref="OrderQueue"/>
		/// </summary>
		public virtual void Start ()
		{
			Starting?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// This is executed when the task is finished; right before this order is taken out of the <see cref="OrderQueue"/>
		/// </summary>
		public virtual void Finish ()
		{
			Finishing?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets a value indicating if this order is finished
		/// </summary>
		public abstract bool Finished { get; }

		/// <summary>
		/// Gets a value indicating whether this order can be cancelled.
		/// </summary>
		/// <value><c>true</c> if this instance can cancel; otherwise, <c>false</c>.</value>
		protected abstract bool CanCancel { get; }

		bool IPrimitiveOrder.CanCancel { get { return CanCancel; } }

		/// <summary>
		/// Occurs when starting.
		/// </summary>
		public event EventHandler Starting;
		/// <summary>
		/// Occurs when finishing.
		/// </summary>
		public event EventHandler Finishing;

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.PrimitiveOrder"/> class.
		/// </summary>
		protected PrimitiveOrder (IUnidad unidad)
		{
			Unidad = unidad;
		}
	}
}