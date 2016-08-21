using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Moggle.IO;

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

		void subsKey ()
		{
			InputManager.AlSerActivado += KeyWatcher;
		}

		void deSubsKey ()
		{
			InputManager.AlSerActivado -= KeyWatcher;
		}

		void KeyWatcher (OpenTK.Input.Key obj)
		{
			switch (obj)
			{
				case OpenTK.Input.Key.Right:
					break;
				default:
					break;
			}
		}
	}
}