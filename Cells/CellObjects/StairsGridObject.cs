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

		/// <summary>
		/// Descarga el contenido gráfico.
		/// Elimina todas las suscripciones a todos los eventos de esta clase
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Cells.CellObjects.StairsGridObject"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Cells.CellObjects.StairsGridObject"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="Cells.CellObjects.StairsGridObject"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Cells.CellObjects.StairsGridObject"/> was occupying.</remarks>
		protected override void Dispose ()
		{
			base.Dispose ();
			AlActivar = null;
		}

		/// <summary>
		/// Invoca el evento <see cref="AlActivar"/>
		/// </summary>
		/// <param name="e">argumentos del evento</param>
		protected virtual void _AlActivar (EventArgs e)
		{
			AlActivar?.Invoke (this, e);
		}

		/// <summary>
		/// Ocurre cuando se activa.
		/// </summary>
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