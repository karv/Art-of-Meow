using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;
using Moggle.Comm;
using Units.Inteligencia;

namespace Screens
{
	public class MapMainScreen : Screen
	{
		
		public override Color BgColor { get { return Color.DarkBlue; } }

		public const int NumChasers = 5;

		public Grid GameGrid;
		public Unidad Jugador;
		public List<IUnidad> UnidadesArtificial = new List<IUnidad> ();
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
				var chaser = new Unidad ();
				chaser.Inteligencia = new ChaseIntelligence (chaser);
				chaser.MapGrid = GameGrid;
				chaser.Inteligencia = new ChaseIntelligence (chaser);
				chaser.Location = GameGrid.RandomPoint ();
				chaser.RecursoHP.Max = 3;
				chaser.RecursoHP.Fill ();
				UnidadesArtificial.Add (chaser);
			}
			Jugador = new Unidad ();
			var Humanintel = new HumanIntelligence (Jugador);
			Jugador.Inteligencia = Humanintel;
			//Components.Add (Humanintel);
			Jugador.MapGrid = GameGrid;
			Jugador.Location = StartingPoint;
			GameGrid.TryCenterOn (Jugador.Location);

		}

		public override void Initialize ()
		{
			GameGrid.AddCellObject (Jugador);
			foreach (var x in UnidadesArtificial)
				GameGrid.AddCellObject (x);

			// Observe que esto debe ser al final, ya que de lo contrario no se inicializarán
			// los nuevos objetos.
			base.Initialize ();
		}

		public override bool RecibirSeñal (KeyboardEventArgs key)
		{
			TeclaPresionada (key);
			base.RecibirSeñal (key);
			return true;
		}

		public override void MandarSeñal (KeyboardEventArgs key)
		{
			var pl = GameGrid.ObjectoActual as Unidad;
			var currobj = pl?.Inteligencia as IReceptorTeclado;
			if (currobj != null && currobj.RecibirSeñal (key))
				return;
			base.MandarSeñal (key);
		}

		/// <summary>
		/// Teclas the presionada.
		/// </summary>
		protected void TeclaPresionada (KeyboardEventArgs keyArg)
		{
			
			var key = keyArg.Key;

			switch (key)
			{
				case Microsoft.Xna.Framework.Input.Keys.Escape:
					Juego.Exit ();
					break;
			/*
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
*/
			}

			/*
			if (actionDir != MovementDirectionEnum.NoMov)
			{
				endTurn = Jugador.MoveOrMelee (actionDir);
				shouldTryRecenter = endTurn;
			}
			*/

			GameGrid.CenterIfNeeded (Jugador);
		}
	}
}