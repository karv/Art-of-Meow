using Moggle.Controles;
using Microsoft.Xna.Framework;
using Units.Recursos;
using System.Linq;
using System.Collections.Generic;

namespace Componentes
{
	public class RecursoView : ListaIconos
	{
		ManejadorRecursos Recursos { get; }

		public RecursoView (IComponentContainerComponent<IGameComponent> cont,
		                    ManejadorRecursos recs)
			: base (cont)
		{
			TopLeft = Vector2.Zero;
			Recursos = recs;
			Iconos = new List<IRelDraw> (Recursos.Enumerar ().OfType<IRelDraw> ());
		}
	}
}