using Moggle.Comm;
using System.Diagnostics;

namespace Cells.CellObjects
{
	public class StairsGridBoject : GridObject, IReceptorTeclado
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
			if (key.Character == '<' || key.Character == '>')
			{
				Activate ();
				return true;
			}
			return false;
		}

		public StairsGridBoject (string texture, Grid grid)
			: base (texture, grid)
		{
		}
	}
}