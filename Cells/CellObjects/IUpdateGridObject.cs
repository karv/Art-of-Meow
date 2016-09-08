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