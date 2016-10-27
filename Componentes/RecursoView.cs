using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Units.Recursos;

namespace Componentes
{
	/// <summary>
	/// Lists the Recursos
	/// </summary>
	public class RecursoView : ListaIconos
	{
		ManejadorRecursos Recursos { get; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Componentes.RecursoView"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Componentes.RecursoView"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[RecursoView: Recursos={0}]", Recursos);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.RecursoView"/> class.
		/// </summary>
		/// <param name="cont">The container where to put this control</param>
		/// <param name="recs">The recursos that this control is going to show</param>
		public RecursoView (IComponentContainerComponent<IControl> cont,
		                    ManejadorRecursos recs)
			: base (cont)
		{
			Recursos = recs;
			Iconos = new List<IDibujable> (Recursos.Enumerate ().OfType<IDibujable> ());
			IconSize = new MonoGame.Extended.Size (64, 12);
			TopLeft = new Point (3, 3);
		}
	}
}