using Moggle.Screens;
using Microsoft.Xna.Framework;
using Cells;
using Units;
using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;
using Moggle.Comm;
using Units.Inteligencia;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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

		public override void LoadContent ()
		{
			base.LoadContent ();
			// Calcular tamaño
			int visCellX = (int)(GetDisplayMode.Width / GameGrid.CellSize.Width);
			int visCellY = (int)(GetDisplayMode.Height / GameGrid.CellSize.Height);
			GameGrid.VisibleCells = new Size (visCellX, visCellY);
			int ScreenOffsX = GetDisplayMode.Width - (int)(GameGrid.CellSize.Width * visCellX);
			GameGrid.ControlTopLeft = new Vector2 (ScreenOffsX / 2, 0);

			buildChasers ();
		}

		void buildChasers ()
		{
			for (int i = 0; i < NumChasers; i++)
			{
				var chaser = new Unidad
				{
					Equipo = 2,
					MapGrid = GameGrid,
					Location = GameGrid.RandomPoint ()
				};
				chaser.Inteligencia = new ChaseIntelligence (chaser);
				chaser.RecursoHP.Max = 3;
				chaser.RecursoHP.Fill ();
				UnidadesArtificial.Add (chaser);
			}

			Jugador = new Unidad
			{
				Equipo = 1,
				Inteligencia = new HumanIntelligence (Jugador),
				MapGrid = GameGrid,
				Location = StartingPoint
			};

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
				case Keys.Escape:
					Juego.Exit ();
					break;
					#if DEBUG
				case Keys.Tab:
					Debug.WriteLine (Jugador.Recursos);
					break;
					#endif
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

		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = new Grid (100, 100, this);
		}
	}
}