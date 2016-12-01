using System;
using System.Collections.Generic;
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
		public override Color? BgColor { get { return Color.Black; } }

		/// <summary>
		/// Resource view manager
		/// </summary>
		public RecursoView _recursoView { get; private set; }

		PlayerInfoControl PlayerInfoControl;

		PlayerKeyListener SpecialKeyListener;

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
					_gameGrid.CameraUnidad = Player;
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

		LogicGrid CurrentGrid;

		void changeGrid (LogicGrid newGrid)
		{
			GridControl.ChangeGrid (newGrid);
			CurrentGrid = newGrid;
		}

		void on_stair_down (object sender, EventArgs e)
		{
			var newGrid = Map.GenerateGrid (Grid.DownMap);
			changeGrid (newGrid);

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
		public override void Draw ()
		{
			Batch.Begin (SpriteSortMode.BackToFront);
			EntreBatches ();
			DrawGridBackground ();
			Batch.End ();
		}

		Texture2D solidTexture;
		Color gridBackgroundColor = Color.DarkRed * 0.15f;

		/// <summary>
		/// Dibuja el fondo de GridControl
		/// </summary>
		protected void DrawGridBackground ()
		{
			Batch.Draw (
				solidTexture,
				destinationRectangle: GridDrawingRectangle,
				color: gridBackgroundColor,
				layerDepth: Depths.Background);
		}

		/// <summary>
		/// Cargar contenido de cada control incluido.
		/// </summary>
		public override void AddAllContent ()
		{
			base.AddAllContent ();
			solidTexture = Content.GetContent<Texture2D> ("pixel");
		}

		/// <summary>
		/// Ciclo de la lógica
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="currentThread">Current thread.</param>
		public override void Update (GameTime gameTime, ScreenThread currentThread)
		{
			base.Update (gameTime, currentThread);
			CurrentGrid?.Update (gameTime);
		}

		/// <summary>
		/// AI players
		/// </summary>
		public List<IUnidad> AIPlayers = new List<IUnidad> ();

		/// <summary>
		/// El área de dibujo del tablero
		/// </summary>
		public static Rectangle GridDrawingRectangle = 
			new Rectangle (30, 30, 1000, 700);

		/// <summary>
		/// Calculates the grid size to make it fir correctly
		/// </summary>
		void generateGridSizes ()
		{
			int visCellX = GridDrawingRectangle.Width / GridControl.CellSize.Width;
			int visCellY = GridDrawingRectangle.Height / GridControl.CellSize.Height;
			GridControl.VisibleCells = new Size (visCellX, visCellY);
			int ScreenOffsX = GridDrawingRectangle.Width - (GridControl.CellSize.Width * visCellX);
			int ScreenOffsY = GridDrawingRectangle.Height - (GridControl.CellSize.Height * visCellY);
			GridControl.ControlTopLeft = new Point (ScreenOffsX / 2, ScreenOffsY / 2) + GridDrawingRectangle.Location;
		}

		/// <summary>
		/// Initializes the human player's control
		/// </summary>
		void inicializarJugador ()
		{
			PlayerInfoControl = new PlayerInfoControl (this, Player);
			PlayerInfoControl.DrawingArea = new Rectangle (
				GridDrawingRectangle.Right, 0, 300, 900);

			_recursoView = new RecursoView (this, Player.Recursos);
			AddComponent (_recursoView);
		}

		/// <summary>
		/// Realiza la inicialización
		/// </summary>
		protected override void DoInitialization ()
		{
			CurrentGrid = GameInitializer.InitializeNewWorld (out Player);
			GridControl = new GridControl (CurrentGrid, this);

			inicializarJugador ();

			base.DoInitialization ();
			generateGridSizes ();

			// Observe que esto debe ser al final, ya que de lo contrario no se inicializarán
			// los nuevos objetos.

			GridControl.TryCenterOn (Player.Location);

			SpecialKeyListener = new PlayerKeyListener (this);
		}

		/// <summary>
		/// Rebice señal del teclado
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="data">Señal tecla</param>
		public override bool RecibirSeñal (Tuple<KeyboardEventArgs, ScreenThread> data)
		{
			var key = data.Item1;
			TeclaPresionada (key);
			base.RecibirSeñal (data);
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
			if (SpecialKeyListener.RecibirSeñal (keyArg))
				return;
			
			var key = keyArg.Key;

			var playerCell = Grid.GetCell (Player.Location);
			foreach (var x in playerCell.EnumerateObjects ().OfType<IReceptor<KeyboardEventArgs>> ())
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

	/// <summary>
	/// Extensiones de Screen
	/// </summary>
	public static class ScreenExt
	{
		/// <summary>
		/// Opciones tipo Dialo
		/// </summary>
		public static readonly ScreenThread.ScreenStackOptions DialogOpt;
		/// <summary>
		/// Opciones default
		/// </summary>
		public static readonly ScreenThread.ScreenStackOptions NewScreen;

		/// <summary>
		/// Ejecuta un screen
		/// </summary>
		public static void Execute (this IScreen scr,
		                            ScreenThread thread,
		                            ScreenThread.ScreenStackOptions opt)
		{
			scr.Initialize ();
			scr.AddContent ();
			scr.Content.Load ();
			scr.InitializeContent ();
			thread.Stack (scr, opt);
		}

		/// <summary>
		/// Ejecuta un screen
		/// </summary>
		public static void Execute (this IScreen scr,
		                            ScreenThreadManager threadMan,
		                            ScreenThread.ScreenStackOptions opt)
		{
			Execute (scr, threadMan.ActiveThread, opt);
		}

		/// <summary>
		/// Ejecuta un screen
		/// </summary>
		public static void Execute (this IScreen scr,
		                            ScreenThread.ScreenStackOptions opt)
		{
			Execute (scr, scr.Juego.ScreenManager, opt);
		}

		static ScreenExt ()
		{
			DialogOpt = new ScreenThread.ScreenStackOptions
			{
				ActualizaBase = false,
				DibujaBase = true
			};
			NewScreen = new ScreenThread.ScreenStackOptions
			{
				ActualizaBase = false,
				DibujaBase = false
			};
		}
	}
}