
namespace Units.Order
{
	/// <summary>
	/// A primitive order where time of termination is determined
	/// </summary>
	public interface ITimedPrimitiveOrder : IPrimitiveOrder
	{
		/// <summary>
		/// Gets the expected time before this order ends
		/// </summary>
		float ExceptedTime { get; }
	}

	/// <summary>
	/// Represents a primitive order in a <see cref="IUnidad"/>'s <see cref="OrderQueue"/>
	/// </summary>
	public interface IPrimitiveOrder
	{
		/// <summary>
		/// Gets the <see cref="IUnidad"/> that is linked to this order
		/// </summary>
		/// <value>The unidad.</value>
		IUnidad Unidad { get; }

		/// <summary>
		/// Gets a value determining if this order is cancelable
		/// </summary>
		bool CanCancel { get; }

		/// <summary>
		/// Gets a value indicating if this order is finished
		/// </summary>
		/// <value><c>true</c> if finished; otherwise, <c>false</c>.</value>
		bool Finished { get; }

		/// <summary>
		/// Updates this order by a given time and returns the unused time
		/// </summary>
		/// <returns><c>true</c>, if the order is finished, <c>false</c> otherwise.</returns>
		/// <param name="gameTime">Time passed</param>
		float PassTime (float gameTime);

		/// <summary>
		/// This method is executed when this order becomes first in the <see cref="OrderQueue"/>
		/// </summary>
		void Start ();

		/// <summary>
		/// This is executed when the task is finished; right before this order is taken out of the <see cref="OrderQueue"/>
		/// </summary>
		void Finish ();
	}
}