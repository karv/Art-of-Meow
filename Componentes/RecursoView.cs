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

		public RecursoView (IComponentContainerComponent<IGameComponent> cont,
		                    ManejadorRecursos recs)
			: base (cont)
		{
			Recursos = recs;
			Iconos = new List<IRelDraw> (Recursos.Enumerar ().OfType<IRelDraw> ());
			IconSize = new MonoGame.Extended.Size (64, 12);
			TopLeft = new Point (3, 3);
		}
	}
}