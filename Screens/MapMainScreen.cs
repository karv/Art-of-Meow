using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;
using System.Collections.Generic;
using OpenTK.Input;

namespace Screens
{
	public class MapMainScreen : Screen
	{
		
		public override Color BgColor { get { return Color.DarkBlue; } }

		public const int NumChasers = 5;

		public Grid GameGrid;
		public UnidadHumano Jugador;
		public List<UnidadArtificial> UnidadesArtificial = new List<UnidadArtificial> ();
		public readonly Point StartingPoint = new Point (50, 50);

		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = new Grid (100, 100, this)
			{
				// TODO: calcularlo automáticamente.
				ControlTopLeft = new Point (20, 0),
				VisibleCells = new Point (55, 27)
			};

			for (int i = 0; i < NumChasers; i++)
			{
				var chaser = new UnidadArtificial ();
				chaser.MapGrid = GameGrid;
				chaser.IA = new ChaseIntelligence (chaser);
				chaser.Location = GameGrid.RandomPoint ();
				UnidadesArtificial.Add (chaser);
			}
			Jugador = new UnidadHumano ();
			Jugador.MapGrid = GameGrid;
			Jugador.Location = StartingPoint;
			GameGrid.TryCenterOn (Jugador.Location);
		}

		public override void Inicializar ()
		{
			base.Inicializar ();
			GameGrid.Include ();
			GameGrid.AddCellObject (Jugador);
			foreach (var x in UnidadesArtificial)
				GameGrid.AddCellObject (x);
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
			var state = Keyboard.GetState ();
			if (!state.IsAnyKeyDown)
				return;
			foreach (var k in typeof (Key).GetEnumValues ())
			{
				var key = (Key)k;
				if (state.IsKeyDown (key))
					TeclaPresionada (key);
			}			
			//foreach (var x in state.IsAnyKeyDown)
			//	TeclaPresionada (x);
		}

		/// <summary>
		/// Teclas the presionada.
		/// </summary>
		/// <param name="key">Key.</param>
		protected override void TeclaPresionada (Key key)
		{
			bool shouldTryRecenter = false;
			bool endTurn = false;
			var actionDir = MovementDirectionEnum.NoMov;
			switch (key)
			{
				case Key.Escape:
					Juego.Exit ();
					break;
				case Key.Down:
				case Key.Keypad2:
					actionDir = MovementDirectionEnum.Down;
					break;
				case Key.Right:
				case Key.Keypad6:
					actionDir = MovementDirectionEnum.Right;
					break;
				case Key.Up:
				case Key.Keypad8:
					actionDir = MovementDirectionEnum.Up;
					break;
				case Key.Left:
				case Key.Keypad4:
					actionDir = MovementDirectionEnum.Left;
					break;
				case Key.Keypad1:
					actionDir = MovementDirectionEnum.DownLeft;
					break;
				case Key.Keypad3:
					actionDir = MovementDirectionEnum.DownRight;
					break;
				case Key.Keypad7:
					actionDir = MovementDirectionEnum.UpLeft;
					break;
				case Key.Keypad9:
					actionDir = MovementDirectionEnum.UpRight;
					break;
			}

			if (actionDir != MovementDirectionEnum.NoMov)
			{
				endTurn = Jugador.MoveOrMelee (actionDir);
				shouldTryRecenter = endTurn;
			}

			if (shouldTryRecenter)
				GameGrid.CenterIfNeeded (Jugador);

			if (endTurn)
				entreTurnos ();
		}

		void entreTurnos ()
		{
			foreach (var x in UnidadesArtificial)
				x.IA.DoAction ();
		}
	}
}