using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Componentes;
using Helper;
using Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Moggle.Comm;
using Moggle.Screens;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;
using Units;
using Units.Recursos;

namespace Screens
{
	/// <summary>
	/// The main screen of the map of the game
	/// </summary>
	public class MapMainScreen : Screen
	{
		/// <summary>
		/// Color de fondo
		/// </summary>
		/// <value>The color of the background.</value>
		public override Color BgColor { get { return Color.DarkBlue; } }

		/// <summary>
		/// Resource view manager
		/// </summary>
		public RecursoView _recursoView { get; private set; }

		/// <summary>
		/// Gets the visual control that displays the buffs
		/// </summary>
		public BuffDisplay _playerHooks { get; private set; }

		/// <summary>
		/// Devuelve el tablero logico.
		/// </summary>
		public LogicGrid Grid { get { return _gameGrid.Grid; } }

		GridControl _gameGrid;

		/// <summary>
		/// Map grid
		/// </summary>
		public GridControl GridControl
		{
			get
			{
				return _gameGrid;
			}
			set
			{
				if (_gameGrid == null)
				{
					_gameGrid = value;
					foreach (var str in _gameGrid.Grid.Objects.OfType<StairsGridObject> ())
						str.AlActivar += on_stair_down;
					return;
				}

				Components.RemoveAll (z => z is GridControl);
				(_gameGrid as IDisposable)?.Dispose ();
				_gameGrid = value;

				_gameGrid.Initialize ();
				AddComponent (_gameGrid);

				// Cargar el posiblemente nuevo contenido
				AddAllContent ();
				Content.Load ();
				InitializeContent ();

			}
		}

		void on_stair_down (object sender, EventArgs e)
		{
			var newGrid = Map.GenerateGrid (Grid.DownMap);
			GridControl.ChangeGrid (newGrid);

			// Recibir la experiencia
			Player.Exp.Flush ();

			// Mover al jugador
			// Poner aquí al jugador
			Player.Location = newGrid.GetRandomEmptyCell ();
			Player.Grid = newGrid;

			foreach (var str in _gameGrid.Grid.Objects.OfType<StairsGridObject> ())
				str.AlActivar += on_stair_down;
			
			Grid.AddCellObject (Player);
		}

		/// <summary>
		/// The human player
		/// </summary>
		public Unidad Player;

		/// <summary>
		/// Dibuja la pantalla
		/// </summary>
		public override void Draw (GameTime gameTime)
		{
			Batch.Begin (SpriteSortMode.BackToFront);
			EntreBatches (gameTime);
			Batch.End ();
		}

		/// <summary>
		/// AI players
		/// </summary>
		public List<IUnidad> AIPlayers = new List<IUnidad> ();

		/// <summary>
		/// Calculates the grid size to make it fir correctly
		/// </summary>
		void generateGridSizes ()
		{
			int visCellX = (int)(GetDisplayMode.Width / GridControl.CellSize.Width);
			int visCellY = (int)(GetDisplayMode.Height / GridControl.CellSize.Height);
			GridControl.VisibleCells = new Size (visCellX, visCellY);
			int ScreenOffsX = GetDisplayMode.Width - (int)(GridControl.CellSize.Width * visCellX);
			GridControl.ControlTopLeft = new Point (ScreenOffsX / 2, 0);
		}

		/// <summary>
		/// Initializes the human player's control
		/// </summary>
		void inicializarJugador ()
		{
			// TEST ing

			// Observe que esta asignación debe ser antes que el hook
			_playerHooks = new BuffDisplay (this, Player)
			{
				MargenInterno = new Moggle.Controles.MargenType
				{
					Top = 1,
					Bot = 1,
					Left = 1,
					Right = 1
				},
				TamañoBotón = new Size (16, 16),
				TipoOrden = Moggle.Controles.Contenedor<Moggle.Controles.IDibujable>.TipoOrdenEnum.ColumnaPrimero,
				Posición = new Point (0, 100)
			};

			_recursoView = new RecursoView (this, Player.Recursos);
			AddComponent (_recursoView);
		}

		/// <summary>
		/// Initializes the grid, and the rest of the controls
		/// </summary>
		public override void Initialize ()
		{
			var lGrid = GameInitializer.InitializeNewWorld (out Player);
			GridControl = new GridControl (lGrid, this);

			inicializarJugador ();
			// REMOVE: ¿Move the Grid initializer ot itself?
			generateGridSizes ();

			// Observe que esto debe ser al final, ya que de lo contrario no se inicializarán
			// los nuevos objetos.
			base.Initialize ();

			GridControl.TryCenterOn (Player.Location);
		}

		/// <summary>
		/// Rebice señal del teclado
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="key">Señal tecla</param>
		public override bool RecibirSeñal (KeyboardEventArgs key)
		{
			TeclaPresionada (key);
			base.RecibirSeñal (key);
			return true;
		}

		/// <summary>
		/// Manda señal de tecla presionada a esta pantalla
		/// </summary>
		/// <param name="key">Tecla de la señal</param>
		public override void MandarSeñal (KeyboardEventArgs key)
		{
			var pl = Grid.CurrentObject as Unidad;
			var currobj = pl?.Inteligencia as IReceptor<KeyboardEventArgs>;
			if (currobj?.RecibirSeñal (key) ?? false)
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
				case Keys.Tab:
					Debug.WriteLine (Player.Recursos);
					break;
				case Keys.I:
					if (Player.Inventory.Any () || Player.Equipment.EnumerateEquipment ().Any ())
					{
						var scr = new EquipmentScreen (this, Player);
						scr.Ejecutar ();
					}
					break;
				case Keys.C:
					GridControl.TryCenterOn (Player.Location);
					break;
				case Keys.S:
					if (Player.Skills.AnyUsable ())
					{
						var scr = new InvokeSkillListScreen (this, Player);
						scr.Ejecutar ();
					}
					break;
			}
			var playerCell = Grid.GetCell (Player.Location);
			foreach (var x in playerCell.Objects.OfType<IReceptor<KeyboardEventArgs>> ())
				x.RecibirSeñal (keyArg);
			GridControl.CenterIfNeeded (Player);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.MapMainScreen"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		/// <param name="map">Map.</param>
		[Obsolete]
		MapMainScreen (Moggle.Game game, Map map)
			: base (game)
		{
			GridControl = new GridControl (map.GenerateGrid (), this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.MapMainScreen"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
		}
	}
}