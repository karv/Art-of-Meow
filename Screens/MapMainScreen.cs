using System.Collections.Generic;
using System.Diagnostics;
using Cells;
using Componentes;
using Items;
using Items.Declarations.Equipment;
using Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moggle.Comm;
using Moggle.Screens;
using MonoGame.Extended;
using MonoGame.Extended.InputListeners;
using Units;
using Units.Buffs;
using Units.Inteligencia;
using Units.Recursos;
using Units.Skills;
using Microsoft.Xna.Framework.Graphics;

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
		/// Map grid
		/// </summary>
		public Grid GameGrid;

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
			int visCellX = (int)(GetDisplayMode.Width / GameGrid.CellSize.Width);
			int visCellY = (int)(GetDisplayMode.Height / GameGrid.CellSize.Height);
			GameGrid.VisibleCells = new Size (visCellX, visCellY);
			int ScreenOffsX = GetDisplayMode.Width - (int)(GameGrid.CellSize.Width * visCellX);
			GameGrid.ControlTopLeft = new Point (ScreenOffsX / 2, 0);
		}

		/// <summary>
		/// Initializes the human player
		/// </summary>
		void inicializarJugador ()
		{
			Player = new Unidad (GameGrid)
			{
				Team = new TeamManager (Color.Red),
				Location = new Point (
					GameGrid.GridSize.Width / 2,
					GameGrid.GridSize.Height / 2)
			};

			Player.Inteligencia = new HumanIntelligence (Player);
			Player.Equipment.EquipItem (ItemFactory.CreateItem (ItemType.Sword) as IEquipment);

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

			var haste = new HasteBuff
			{
				SpeedDelta = 10,
				Duración = 1
			};
			haste.Initialize ();
			Player.Buffs.Hook (haste);
			var spd = Player.Recursos.ValorRecurso (ConstantesRecursos.Velocidad);
			System.Console.WriteLine (spd);

			var sword = ItemFactory.CreateItem (ItemType.Sword) as Sword;
			//Jugador.Equipment.EquipItem (sword);
			Player.Inventory.Add (sword);
			Player.Inventory.Add (ItemFactory.CreateItem (ItemType.Sword));
			Player.Inventory.Add (ItemFactory.CreateItem (ItemType.Sword));
			var healSkill = new SelfHealSkill ();
			Player.Skills.Skills.Add (healSkill);
			healSkill.Initialize ();

			_recursoView = new RecursoView (this, Player.Recursos);
		}

		/// <summary>
		/// Initializes the grid, and the rest of the controls
		/// </summary>
		public override void Initialize ()
		{
			// REMOVE: ¿Move the Grid initializer ot itself?
			generateGridSizes ();
			inicializarJugador ();

			GameGrid.AddCellObject (Player);

			// Observe que esto debe ser al final, ya que de lo contrario no se inicializarán
			// los nuevos objetos.
			base.Initialize ();
			GameGrid.TryCenterOn (Player.Location);
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
			var pl = GameGrid.CurrentObject as Unidad;
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
				case Keys.Tab:
					Debug.WriteLine (Player.Recursos);
					break;
				case Keys.I:
					if (Player.Inventory.Any ())
					{
						var scr = new EquipmentScreen (this, Player);
						scr.Ejecutar ();
					}
					break;
				case Keys.C:
					GameGrid.TryCenterOn (Player.Location);
					break;
				case Keys.S:
					if (Player.Skills.AnyVisible)
					{
						var scr = new InvokeSkillListScreen (this, Player);
						scr.Ejecutar ();
					}
					break;
			}
			GameGrid.CenterIfNeeded (Player);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.MapMainScreen"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		public MapMainScreen (Moggle.Game game)
			: base (game)
		{
			GameGrid = Map.GenerateGrid (@"Maps/base.map", this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.MapMainScreen"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		/// <param name="map">Map.</param>
		public MapMainScreen (Moggle.Game game, Map map)
			: base (game)
		{
			GameGrid = map.GenerateGrid (this);
		}
	}
}