using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;

namespace Screens
{
	public class MapMainScreen : Screen
	{
		
		public override Color BgColor
		{
			get
			{
				return Color.DarkBlue;
			}
		}

		public Grid GameGrid;
		public UnidadHumano Jugador;
		public readonly Point StartingPoint = new Point (0, 0);

		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = new Grid (100, 100, this);
			GameGrid.ControlTopLeft = new Point (100, 100);

			Jugador = new UnidadHumano (Content);
		}

		public override void Inicializar ()
		{
			base.Inicializar ();
			GameGrid.Include ();
			GameGrid.AddCellObject (StartingPoint, Jugador);
		}

		public override void LoadContent ()
		{
			base.LoadContent ();
			Jugador.CellObject.LoadContent ();
		}

		protected override void TeclaPresionada (OpenTK.Input.Key key)
		{
			bool shouldTryRecenter = false;
			switch (key)
			{
				case OpenTK.Input.Key.Escape:
					Juego.Exit ();
					break;
				case OpenTK.Input.Key.Down:
				case OpenTK.Input.Key.Keypad2:
					GameGrid.MoveCellObject (Jugador.CellObject, MovementDirectionEnum.Down);
					shouldTryRecenter = true;
					break;
			}

			if (shouldTryRecenter)
				GameGrid.CenterIfNeeded (Jugador.CellObject);
		}
	}
}