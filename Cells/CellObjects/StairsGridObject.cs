using System.Diagnostics;
using Moggle.Comm;
using Moggle.Controles;
using System;

namespace Cells.CellObjects
{
	/// <summary>
	/// Escaleras como objeto de Grid
	/// </summary>
	public class StairsGridObject : GridObject, IReceptorTeclado, IActivable
	{
		/// <summary>
		/// Usa la escalera
		/// </summary>
		void IActivable.Activar ()
		{
			Debug.WriteLine ("Stairs");
			_AlActivar (EventArgs.Empty);
		}

		bool IReceptorTeclado.RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs key)
		{
			if (key.Character == ',' || key.Character == '>')
			{
				Debug.WriteLine ("Stairs");
				_AlActivar (EventArgs.Empty);
				return true;
			}
			return false;
		}

		protected override void Dispose ()
		{
			base.Dispose ();
			AlActivar = null;
		}

		protected virtual void _AlActivar (EventArgs e)
		{
			AlActivar?.Invoke (this, e);
		}

		public event EventHandler AlActivar;

		/// <summary>
		/// </summary>
		/// <param name="texture">Texture.</param>
		/// <param name="grid">Grid.</param>
		public StairsGridObject (string texture, LogicGrid grid)
			: base (texture, grid)
		{
		}
	}
}