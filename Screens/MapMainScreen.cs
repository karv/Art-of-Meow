using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;
using System.Collections.Generic;
using OpenTK.Input;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;

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
				ControlTopLeft = new Vector2 (20, 0),
				VisibleCells = new Size (55, 27)
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

		public override void Initialize ()
		{
			base.Initialize ();
			Components.Add (GameGrid);

			GameGrid.AddCellObject (Jugador);
			foreach (var x in UnidadesArtificial)
				GameGrid.AddCellObject (x);
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
			/*
			var state = Keyboard.GetState ();
			if (!state.IsAnyKeyDown)
				return;
			foreach (var k in typeof (Key).GetEnumValues ())
			{
				var key = (Key)k;
				if (state.IsKeyDown (key))
					TeclaPresionada (new KeyboardEventArgs (key, state));
				//TeclaPresionada (key);
			}			
			//foreach (var x in state.IsAnyKeyDown)
			//	TeclaPresionada (x);
			*/
		}

		public override bool RecibirSeñal (KeyboardEventArgs key)
		{
			TeclaPresionada (key);
			return true;
		}

		/// <summary>
		/// Teclas the presionada.
		/// </summary>
		protected void TeclaPresionada (KeyboardEventArgs keyArg)
		{
			var key = keyArg.Key;
			bool shouldTryRecenter = false;
			bool endTurn = false;
			var actionDir = MovementDirectionEnum.NoMov;
			switch (key)
			{
				case Microsoft.Xna.Framework.Input.Keys.Escape:
					Juego.Exit ();
					break;
				case Microsoft.Xna.Framework.Input.Keys.Down:
				case Microsoft.Xna.Framework.Input.Keys.NumPad2:
					actionDir = MovementDirectionEnum.Down;
					break;
				case Microsoft.Xna.Framework.Input.Keys.Right:
				case Microsoft.Xna.Framework.Input.Keys.NumPad6:
					actionDir = MovementDirectionEnum.Right;
					break;
				case Microsoft.Xna.Framework.Input.Keys.Up:
				case Microsoft.Xna.Framework.Input.Keys.NumPad8:
					actionDir = MovementDirectionEnum.Up;
					break;
				case Microsoft.Xna.Framework.Input.Keys.Left:
				case Microsoft.Xna.Framework.Input.Keys.NumPad4:
					actionDir = MovementDirectionEnum.Left;
					break;
				case Microsoft.Xna.Framework.Input.Keys.NumPad1:
					actionDir = MovementDirectionEnum.DownLeft;
					break;
				case Microsoft.Xna.Framework.Input.Keys.NumPad3:
					actionDir = MovementDirectionEnum.DownRight;
					break;
				case Microsoft.Xna.Framework.Input.Keys.NumPad7:
					actionDir = MovementDirectionEnum.UpLeft;
					break;
				case Microsoft.Xna.Framework.Input.Keys.NumPad9:
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