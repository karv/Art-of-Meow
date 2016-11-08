using System.Diagnostics;
using Moggle.Comm;
using Screens;
using Maps;

namespace Cells.CellObjects
{
	/// <summary>
	/// Escaleras como objeto de Grid
	/// </summary>
	public class StairsGridObject : GridObject, IReceptorTeclado
	{
		/// <summary>
		/// Usa la escalera
		/// </summary>
		public void Activate ()
		{
			Debug.WriteLine ("Stairs!");
			var scr = (MapMainScreen)Grid.Screen;
			var newGrid = Map.GenerateGrid (Grid.DownMap, scr);
			// Mover al jugador al nuevo grid

			scr.GameGrid = newGrid;
		}

		bool IReceptorTeclado.RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs key)
		{
			if (key.Character == ',' || key.Character == '>')
			{
				Activate ();
				return true;
			}
			return false;
		}

		/// <summary>
		/// </summary>
		/// <param name="texture">Texture.</param>
		/// <param name="grid">Grid.</param>
		public StairsGridObject (string texture, Grid grid)
			: base (texture, grid)
		{
		}
	}
}