using Moggle.Screens;
using Microsoft.Xna.Framework;

namespace Art_of_Meow
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
		}

		public override void Inicializar ()
		{
			base.Inicializar ();
			GameGrid.Include ();
		}
	}
}