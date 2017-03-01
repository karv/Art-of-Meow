using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Units;
using Units.Inteligencia;

namespace Componentes
{
	/// <summary>
	/// The Grid system
	/// </summary>
	public class GridControl : DSBC
	{
		#region Data & Internal

		const string damageFont = "Fonts//damage";

		#endregion

		#region Grid

		/// <summary>
		/// Devuelve el tablero lógico
		/// </summary>
		public LogicGrid Grid { get; private set; }

		/// <summary>
		/// Cambia el tablero actual, liberando completamente al anterior e inicializando el nuevo
		/// </summary>
		/// <param name="newGrid">Nuevo tablero lógico</param>
		public void ChangeGrid (LogicGrid newGrid)
		{
			Grid.Dispose ();
			Grid = newGrid;
			reInitialize ();
		}

		#endregion

		#region Initializers

		void reInitialize ()
		{
			Initialize ();
			var cont = Screen.Content;
			LoadContent (cont);
		}

		#endregion

		#region Objects & Units

		IEnumerable<Unidad> AIPlayers
		{ 
			get { return _objects.OfType<Unidad> ().Where (z => !(z.Inteligencia is HumanIntelligence)); }
		}

		IEnumerable<IGridObject> _objects { get { return Grid.Objects; } }

		#endregion

		#region Control size and location

		Size _gameSize = new Size (1200, 480);

		Size _cellSize;

		Size _visibleCells;

		/// <summary>
		/// The size of a cell (Draw)
		/// </summary>
		public Size CellSize
		{
			get
			{
				return _cellSize;
			}
			set
			{
				if (_gameSize.Width % value.Width == 0 && _gameSize.Height % value.Height == 0)
				{
					_cellSize = value;
					_visibleCells = new Size (_gameSize.Width / value.Width, _gameSize.Height / value.Height);
				}
			}
		}

		/// <summary>
		/// Gets the number of visible cells
		/// </summary>
		public Size VisibleCells
		{
			get
			{
				return _visibleCells;
			}
			set
			{
				if (_gameSize.Width % value.Width == 0 && _gameSize.Height % value.Height == 0)
				{
					_visibleCells = value;
					_cellSize = new Size (_gameSize.Width / value.Width, _gameSize.Height / value.Height);
				}
			}
		}

		/// <summary>
		/// Celda de _data que se muestra en celda visible (0,0)
		/// </summary>
		public Point CurrentVisibleTopLeft = Point.Zero;

		/// <summary>
		/// Posición top left del control.
		/// </summary>
		public Point ControlTopLeft = Point.Zero;

		/// <summary>
		/// Gets the size of this grid, as a <see cref="IControl"/>
		/// </summary>
		/// <value>The size of the control.</value>
		public Size ControlSize
		{
			get
			{
				return new Size (VisibleCells.Width * CellSize.Width,
					VisibleCells.Height * CellSize.Height);
			}
		}

		/// <summary>
		/// Gets the bounds
		/// </summary>
		/// <value>The bounds.</value>
		public RectangleF Bounds
		{
			get
			{
				return new RectangleF (ControlTopLeft.ToVector2 (), ControlSize);
			}
		}


		/// <summary>
		/// Devuelve el límite gráfico del control.
		/// </summary>
		/// <returns>The bounds.</returns>
		protected override IShapeF GetBounds ()
		{
			return Bounds;
		}

		#endregion

		#region Draw

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		protected override void Draw ()
		{
			//var bat = Screen.
			//bat.Begin (SpriteSortMode.BackToFront);
			var bat = Screen.Batch;

			var box = GetVisibilityBox ();
			var intel = CameraUnidad.Inteligencia as HumanIntelligence;
			for (int ix = box.Left; ix <= box.Right; ix++)
			{
				for (int iy = box.Top; iy <= box.Bottom; iy++)
				{
					var p = new Point (ix, iy);
					var rectOutput = new Rectangle (CellSpotLocation (p), CellSize);
					if (CameraUnidad?.CanSee (p) ?? true)
						Grid [p].Draw (bat, rectOutput);
					else
					{
						// Dejar que la memoria dibuje
						var dCell = intel.Memory [p];
						dCell.Draw (bat, rectOutput);
					}
				}
			}
		}

		#endregion

		#region Memory

		/// <summary>
		/// Agrega el contenido a la biblitoeca
		/// </summary>
		protected override void LoadContent (ContentManager manager)
		{
			base.LoadContent (manager);
			foreach (var x in _objects)
				x.LoadContent (manager);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="GridControl"/> object.
		/// Unsusbribe to Grid's events; so it can be collected by GC.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="GridControl"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="GridControl"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="GridControl"/> so the garbage
		/// collector can reclaim the memory that the <see cref="GridControl"/> was occupying.</remarks>
		protected override void Dispose ()
		{
			Grid.ObjectAdded -= itemAdded;
			base.Dispose ();
		}


		#endregion

		#region Behavior

		/// <summary>
		/// Update lógico
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public override void Update (GameTime gameTime)
		{
		}

		/// <summary>
		/// Se ejecuta antes del ciclo, pero después de saber un poco sobre los controladores.
		/// No invoca LoadContent por lo que es seguro agregar componentes
		/// </summary>
		public override void Initialize ()
		{
			if (!IsInitialized)
				VisibleCells = new Size (50, 20);
			base.Initialize ();
			Grid.ObjectAdded += itemAdded;
		}

		#endregion

		#region Camera

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="p">coordenadas del spot</param>
		public Point CellSpotLocation (Point p)
		{
			var _x = ControlTopLeft.X + CellSize.Width * (p.X - CurrentVisibleTopLeft.X);
			var _y = ControlTopLeft.Y + CellSize.Height * (p.Y - CurrentVisibleTopLeft.Y);
			return new Point (_x, _y);
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="x">Coordenada X</param>
		/// <param name="y">Coordenada Y</param>
		public Point CellSpotLocation (int x, int y)
		{
			return CellSpotLocation (new Point (x, y));
		}

		/// <summary>
		/// Devuelve o establece la unidad que debe de seguir el control
		/// </summary>
		public Unidad CameraUnidad { get; set; }

		/// <summary>
		/// Devuelve el punto de visibilidad (ie, la localización de <see cref="CameraUnidad"/>
		/// </summary>
		public Point VisibilityPoint
		{ 
			get{ return CameraUnidad.Location; }
		}

		/// <summary>
		/// Centra el campo visible en la dirección de una celda.
		/// </summary>
		/// <param name="p">P.</param>
		public void TryCenterOn (Point p)
		{
			var left = p.X - VisibleCells.Width / 2;
			var top = p.Y - VisibleCells.Height / 2;
			CurrentVisibleTopLeft = new Point (left, top);
		}

		/// <summary>
		/// Determina si una dirección de celda es visible actualmente.
		/// </summary>
		/// <param name="p">Dirección de celda.</param>
		public bool IsVisible (Point p)
		{
			return GetVisibilityBox ().Contains (p);
		}

		/// <summary>
		/// Gets a rectangle representing the edges (mod grid) of the view
		/// </summary>
		public Rectangle GetVisibilityBox ()
		{
			return new Rectangle (CurrentVisibleTopLeft, VisibleCells);
		}

		/// <summary>
		/// The size of the edge.
		/// Objects outside this area are considered as "centered enough"
		/// </summary>
		static Size _edgeSize = new Size (6, 4);

		/// <summary>
		/// Centers the view on a given object, if it is not centered enough.
		/// </summary>
		public void CenterIfNeeded (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");

			if (!IsInCenter (obj.Location, _edgeSize))
				TryCenterOn (obj.Location);
		}

		/// <summary>
		/// Determines if a given point is centered enough
		/// </summary>
		/// <returns><c>true</c> if the given point is centered enough; otherwise, <c>false</c>.</returns>
		/// <param name="p">Grid-based point</param>
		/// <param name="edge_size">Size of the "not centered" area</param>
		/// <seealso cref="CenterIfNeeded"/>
		/// <seealso cref="_edgeSize"/>
		bool IsInCenter (Point p, Size edge_size)
		{
			var edge = GetVisibilityBox ();
			edge.Inflate (-edge_size.Width, -edge_size.Height);
			return edge.Contains (p);
		}

		/// <summary>
		/// If possible, zooms in the camera
		/// </summary>
		public void ZoomIn ()
		{
			if (VisibleCells.Width % 2 == 0 && VisibleCells.Height % 2 == 0)
			{
				VisibleCells = new Size (VisibleCells.Width / 2, VisibleCells.Height / 2);
				TryCenterOn ((Screen as Screens.MapMainScreen).Player.Location);
			}
		}

		/// <summary>
		/// If possible, zooms out the camera
		/// </summary>
		public void ZoomOut ()
		{
			if (CellSize.Width % 2 == 0 && CellSize.Height % 2 == 0)
			{
				CellSize = new Size (CellSize.Width / 2, CellSize.Height / 2);
				TryCenterOn ((Screen as Screens.MapMainScreen).Player.Location);
			}
		}

		#endregion

		#region Events

		void itemAdded (object sender, IGridObject e)
		{
			if (e.Texture == null)
			{
				// Initialize and load content
				e.Initialize ();
				e.LoadContent (Screen.Content);
			}
		}

		#endregion

		#region Ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="GridControl"/> class.
		/// </summary>
		/// <param name="grid">El tablero lógico</param>
		/// <param name="scr">Screen where this grid belongs to</param>
		public GridControl (LogicGrid grid, Moggle.Screens.IScreen scr)
			: base (scr)
		{
			Grid = grid;
		}

		#endregion
	}
}