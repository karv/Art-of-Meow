namespace Cells.CellObjects
{
	/// <summary>
	/// Es un <see cref="IGridObject"/> que interactúa respecto al tiempo del juego.
	/// </summary>
	public interface IUpdateGridObject : IGridObject
	{
		/// <summary>
		/// Gets the time for the next action.
		/// </summary>
		float NextActionTime { get; }

		/// <summary>
		/// Gets a value indicating whether this object is ready to <see cref="Execute"/>
		/// </summary>
		bool IsReady { get; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Cells.CellObjects.IUpdateGridObject"/> is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		bool Enabled { get; }

		/// <summary>
		/// Execute this
		/// </summary>
		void Execute ();

		/// <summary>
		/// Pass time
		/// </summary>
		/// <param name="time">Time.</param>
		void PassTime (float time);
	}
}