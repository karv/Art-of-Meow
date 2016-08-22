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
		public readonly Point StartingPoint = new Point (1, 1);

		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = new Grid (100, 100, this);
			GameGrid.ControlTopLeft = new Point (100, 100);

			Jugador = new UnidadHumano (Content);
			Jugador.Location = StartingPoint;
		}

		public override void Inicializar ()
		{
			base.Inicializar ();
			GameGrid.Include ();
			GameGrid.AddCellObject (Jugador);
		}

		public override void LoadContent ()
		{
			base.LoadContent ();
			GameGrid.LoadContent ();
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
					GameGrid.MoveCellObject (
						Jugador, 
						MovementDirectionEnum.Down);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Right:
				case OpenTK.Input.Key.Keypad6:
					GameGrid.MoveCellObject (
						Jugador, 
						MovementDirectionEnum.Right);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Up:
				case OpenTK.Input.Key.Keypad8:
					GameGrid.MoveCellObject (
						Jugador, 
						MovementDirectionEnum.Up);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Left:
				case OpenTK.Input.Key.Keypad4:
					GameGrid.MoveCellObject (
						Jugador, 
						MovementDirectionEnum.Left);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad1:
					GameGrid.MoveCellObject (
						Jugador,
						MovementDirectionEnum.DownLeft);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad3:
					GameGrid.MoveCellObject (
						Jugador,
						MovementDirectionEnum.DownRight);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad7:
					GameGrid.MoveCellObject (
						Jugador, 
						MovementDirectionEnum.UpLeft);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad9:
					GameGrid.MoveCellObject (
						Jugador,
						MovementDirectionEnum.UpRight);
					shouldTryRecenter = true;
					break;
			}

			if (shouldTryRecenter)
				GameGrid.CenterIfNeeded (Jugador);
		}
	}
}