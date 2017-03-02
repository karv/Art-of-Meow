using System;
using System.Collections.Generic;
using System.Linq;
using AoM;
using Cells;
using Componentes;
using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Moggle.Comm;
using Moggle.Screens;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;
using Units;
using Units.Inteligencia;

namespace Screens
{
	/// <summary>
	/// The main screen of the map of the game
	/// </summary>
	public class MapMainScreen : Screen
	{
		#region Properties

		/// <summary>
		/// Color de fondo
		/// </summary>
		/// <value>The color of the background.</value>
		public override Color? BgColor { get { return Color.Black; } }

		#endregion

		#region Controls, components and services

		PlayerInfoControl PlayerInfoControl;

		PlayerKeyListener SpecialKeyListener;

		#endregion

		#region Data & internal

		/// <summary>
		/// AI players
		/// </summary>
		public List<IUnidad> AIPlayers = new List<IUnidad> ();

		/// <summary>
		/// The human player
		/// </summary>
		public Unidad Player;

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
					return;
				}
				
				Components.RemoveAll (z => z is GridControl);
				(_gameGrid as IDisposable)?.Dispose ();
				_gameGrid = value;
				
				_gameGrid.Initialize ();
				AddComponent (_gameGrid);
				
				// Cargar el posiblemente nuevo contenido
				LoadAllContent ();
			}
		}

		#endregion

		#region Dynamic

		/// <summary>
		/// Cambia de grid y posición el jugador (y a la pantalla)
		/// </summary>
		/// <param name="newGrid">Nueva posición en el mundo</param>
		public void ChangeGrid (WorldLocation newGrid)
		{
			Player.Exp.Flush ();
			Grid.RemoveObject (Player);
			GridControl.ChangeGrid (newGrid.Grid);
			Player.Location = newGrid.GridPoint;
			Player.Grid = Grid;
			Grid.AddCellObject (Player);
			GridControl.TryCenterOn (Player.Location);
			PlayerInfoControl.ReloadStats ();
			Player.Reinitialize ();

			// update the minimap
			PlayerInfoControl.Minimap.DisplayingGrid = (Player.Inteligencia as HumanIntelligence).Memory;
		}

		#endregion

		#region Drawing

		void recenterCameraOnPlayer (object sender, EventArgs e)
		{
			GridControl.CenterIfNeeded (Player);
		}

		/// <summary>
		/// El área de dibujo del tablero
		/// </summary>
		public static Rectangle GridDrawingRectangle = 
			new Rectangle (30, 30, 1000, 700);

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
		[Obsolete]
		protected void DrawGridBackground ()
		{
			Batch.Draw (
				solidTexture,
				destinationRectangle: GridDrawingRectangle,
				color: gridBackgroundColor,
				layerDepth: Depths.Background);
		}

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

		#endregion

		#region Memory

		/// <summary>
		/// Cargar contenido de cada control incluido.
		/// </summary>
		public override void LoadAllContent ()
		{
			solidTexture = (Juego as Juego).SimpleTextureGenerator.SolidTexture (
				new Size (1, 1),
				Color.White);
			base.LoadAllContent ();
		}

		#endregion

		#region Screen logic

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
		}

		/// <summary>
		/// Ciclo de la lógica
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="currentThread">Current thread.</param>
		public override void Update (GameTime gameTime, ScreenThread currentThread)
		{
			base.Update (gameTime, currentThread);
			Grid?.Update (gameTime);
		}

		#endregion

		#region Initializers

		/// <summary>
		/// Initializes the human player's control
		/// </summary>
		void inicializarJugador ()
		{
			PlayerInfoControl = new PlayerInfoControl (this, Player);
			PlayerInfoControl.DrawingArea = new Rectangle (
				GridDrawingRectangle.Right, 0, 300, 900);

			// Evento de movimiento para centrar cámara
			Player.OnRelocation += recenterCameraOnPlayer;
		}

		/// <summary>
		/// Realiza la inicialización
		/// </summary>
		protected override void DoInitialization ()
		{
			var grd = GameInitializer.InitializeNewWorld (out Player);
			GridControl = new GridControl (grd, this);

			inicializarJugador ();

			base.DoInitialization ();
			generateGridSizes ();

			// Observe que esto debe ser al final, ya que de lo contrario no se inicializarán
			// los nuevos objetos.

			GridControl.TryCenterOn (Player.Location);

			SpecialKeyListener = new PlayerKeyListener (this);

			LoadAllContent ();
		}

		#endregion

		#region Ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.MapMainScreen"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
		}

		#endregion
	}
}