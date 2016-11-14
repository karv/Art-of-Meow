using Cells;
using Componentes;
using Moggle.Screens;

namespace Screens
{
	public class SelectableGridControl : GridControl
	{
		public SelectableGridControl (LogicGrid grid, IScreen scr)
			: base (grid, scr)
		{
		}
	}

	public class SelectTargetScreen : DialScreen
	{
		// No dibujar base, hay que dibujar otro tipo-GridControl más apropiado
		public override bool DibujarBase { get { return false; } }

		public LogicGrid Grid { get; }

		public SelectableGridControl GridSelector { get; }

		public override void Initialize ()
		{
			base.Initialize ();

			// Poner a GridSelector donde debe
			// TODO?
		}

		public SelectTargetScreen (IScreen baseScreen, LogicGrid grid)
			: base (baseScreen.Juego, baseScreen)
		{
			Grid = grid;
			GridSelector = new SelectableGridControl (Grid, this);
			AddComponent (GridSelector);
		}
		
	}
}