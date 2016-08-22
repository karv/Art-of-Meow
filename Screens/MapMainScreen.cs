using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;
using Microsoft.Xna.Framework.Graphics;

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
					GameGrid.MoveCellObject (
						Jugador.CellObject, 
						MovementDirectionEnum.Down);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Right:
				case OpenTK.Input.Key.Keypad6:
					GameGrid.MoveCellObject (
						Jugador.CellObject, 
						MovementDirectionEnum.Right);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Up:
				case OpenTK.Input.Key.Keypad8:
					GameGrid.MoveCellObject (
						Jugador.CellObject, 
						MovementDirectionEnum.Up);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Left:
				case OpenTK.Input.Key.Keypad4:
					GameGrid.MoveCellObject (
						Jugador.CellObject, 
						MovementDirectionEnum.Left);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad1:
					GameGrid.MoveCellObject (
						Jugador.CellObject,
						MovementDirectionEnum.DownLeft);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad3:
					GameGrid.MoveCellObject (
						Jugador.CellObject,
						MovementDirectionEnum.DownRight);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad7:
					GameGrid.MoveCellObject (
						Jugador.CellObject, 
						MovementDirectionEnum.UpLeft);
					shouldTryRecenter = true;
					break;
				case OpenTK.Input.Key.Keypad9:
					GameGrid.MoveCellObject (
						Jugador.CellObject,
						MovementDirectionEnum.UpRight);
					shouldTryRecenter = true;
					break;
			}

			if (shouldTryRecenter)
				GameGrid.CenterIfNeeded (Jugador.CellObject);
		}

	}
}