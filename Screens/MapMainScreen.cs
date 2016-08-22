using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;
using System.Collections.Generic;
using OpenTK.Platform.X11;

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

		public const int NumChasers = 30;

		public Grid GameGrid;
		public UnidadHumano Jugador;
		public List<UnidadArtificial> UnidadesArtificial = new List<UnidadArtificial> ();
		public readonly Point StartingPoint = new Point (1, 1);

		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = new Grid (100, 100, this);
			GameGrid.ControlTopLeft = new Point (100, 100);

			for (int i = 0; i < NumChasers; i++)
			{
				var chaser = new UnidadArtificial (Content);
				chaser.IA = new ChaseIntelligence (GameGrid, chaser);
				chaser.Location = GameGrid.RandomPoint ();
				UnidadesArtificial.Add (chaser);
			}
			Jugador = new UnidadHumano (Content);
			Jugador.Location = StartingPoint;
		}

		public override void Inicializar ()
		{
			base.Inicializar ();
			GameGrid.Include ();
			GameGrid.AddCellObject (Jugador);
			foreach (var x in UnidadesArtificial)
				GameGrid.AddCellObject (x);
		}

		public override void LoadContent ()
		{
			base.LoadContent ();
			GameGrid.LoadContent ();
		}

		protected override void TeclaPresionada (OpenTK.Input.Key key)
		{
			bool shouldTryRecenter = false;
			bool endTurn = false;
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

			endTurn = shouldTryRecenter;
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