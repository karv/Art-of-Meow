using System;

namespace Cells.CellObjects
{

	/// <summary>
	/// Es un <see cref="IGridObject"/> que interactúa respecto al tiempo del juego.
	/// </summary>
	public interface IUpdateGridObject : IGridObject
	{
		TimeSpan NextActionTime { get; }

		void Execute ();

		void PassTime (TimeSpan time);
	}
}