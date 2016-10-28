
namespace Units.Order
{
	/// <summary>
	/// This order disables a unidad from taking other orders during a fixed time
	/// </summary>
	public class CooldownOrder : PrimitiveOrder, ITimedPrimitiveOrder
	{
		/// <summary>
		/// Time until this order if finished
		/// </summary>
		public float Time { get; protected set; }

		/// <summary>
		/// Updates this order by a given time
		/// </summary>
		/// <param name="gameTime">Time passed</param>
		public override void PassTime (float gameTime)
		{
			Time -= gameTime;
		}

		/// <summary>
		/// Gets a value indicating if this order is finished
		/// </summary>
		public override bool Finished { get { return Time <= 0; } }

		/// <summary>
		/// Gets a value indicating whether this order can be cancelled.
		/// </summary>
		/// <value><c>true</c> if this instance can cancel; otherwise, <c>false</c>.</value>
		protected override bool CanCancel { get { return false; } }

		float ITimedPrimitiveOrder.ExceptedTime { get { return Time; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Order.CooldownOrder"/> class.
		/// </summary>
		/// <param name="unidad">User</param>
		/// <param name="initialTime">Initial time.</param>
		public CooldownOrder (IUnidad unidad, float initialTime)
			: base (unidad)
		{
			Time = initialTime;
		}
	}
}