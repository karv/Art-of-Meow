using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Units.Recursos;

namespace Componentes
{
	public class RecursoView : ListaIconos
	{
		ManejadorRecursos Recursos { get; }

		public override string ToString ()
		{
			return string.Format ("[RecursoView: Recursos={0}]", Recursos);
		}

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