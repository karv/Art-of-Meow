using Moggle.Comm;
using System.Diagnostics;

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