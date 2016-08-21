using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;

namespace Screens
{
	public class SomeScreen : Screen
	{
		public override Color BgColor
		{
			get
			{
				return Color.Blue;
			}
		}

		public Grid GameGrid;

		public SomeScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = new Grid (100, 100, this);
			GameGrid.ControlTopLeft = new Point (100, 100);
		}

		public override void Inicializar ()
		{
			base.Inicializar ();
			GameGrid.Include ();
		}
	}
}