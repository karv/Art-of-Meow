using System;
using System.Collections.Generic;
using AoM;
using Cells.CellObjects;
using Cells.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Units;

namespace Cells
{
	/// <summary>
	/// The Griod system
	/// </summary>
	public class Grid : DSBC, IComponentContainerComponent<IGridObject>
	{
		/// <summary>
		/// Devuelve o establece el archivo de generador mapa que se usará como próximo nivel
		/// </summary>
		public string DownMap { get; set; }

		/// <summary>
		/// Builds a <see cref="Cell"/> of the current state of a given point
		/// </summary>
		/// <param name="p">Grid-wise point of the cell</param>
		public Cell GetCell (Point p)
		{
			return new Cell (this, p);
		}

		/// <summary>
		/// Gets the collection of the grid objects
		/// </summary>
		public ICollection<IGridObject> Objects
		{
			get
			{
				return _objects;
			}
		}

		readonly HashSet<IGridObject> _objects = new HashSet<IGridObject> ();
		readonly CollisionSystem _collisionSystem;

		readonly Random _r = new Random ();

		/// <summary>
		/// The size of a cell (Draw)
		/// </summary>
		public SizeF CellSize = new SizeF (24, 24);

		/// <summary>
		/// The time manager.
		/// </summary>
		public GameTimeManager TimeManager;

		/// <summary>
		/// Gets the dimentions lenght of the world, in Grid-long
		/// </summary>
		public Size GridSize { get; }

		/// <summary>
		/// Gets the currently active object
		/// </summary>
		public IUpdateGridObject CurrentObject { get { return TimeManager.Actual; } }

		/// <summary>
		/// enumera las celdas de contorno.
		/// </summary>
		IEnumerable<Point> contorno ()
		{
			for (int i = 0; i < GridSize.Width; i++)
			{
				yield return (new Point (i, 0));
				yield return (new Point (i, GridSize.Height - 1));
			}
			for (int i = 1; i < GridSize.Height - 1; i++)
			{
				yield return (new Point (0, i));
				yield return (new Point (GridSize.Width - 1, i));
			}
		}

		/// <summary>
		/// Gets a random point of a cell inside this grid
		/// </summary>
		public Point RandomPoint ()
		{
			var size = GridSize;
			return new Point (
				1 + _r.Next (size.Width - 2),
				1 + _r.Next (size.Height - 2));
		}

		/// <summary>
		/// Agrega un objeto al grid.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void AddCellObject (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			Objects.Add (obj);
		}

		/// <summary>
		/// Removes an object from the grid
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public void RemoveObject (IGridObject obj)
		{
			Objects.Remove (obj);
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
		/// Gets the number of visible cells
		/// </summary>
		public Size VisibleCells = new Size (50, 20);

		/// <summary>
		/// Gets the size of this grid, as a <see cref="IControl"/>
		/// </summary>
		/// <value>The size of the control.</value>
		public Size ControlSize
		{
			get
			{
				return new Size ((int)(VisibleCells.Width * CellSize.Width),
					(int)(VisibleCells.Height * CellSize.Height));
			}
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="p">coordenadas del spot</param>
		public Point CellSpotLocation (Point p)
		{
			var _x = (int)(ControlTopLeft.X + CellSize.Width * (p.X - CurrentVisibleTopLeft.X));
			var _y = (int)(ControlTopLeft.Y + CellSize.Height * (p.Y - CurrentVisibleTopLeft.Y));
			return new Point (_x, _y);
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
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="x">Coordenada X</param>
		/// <param name="y">Coordenada Y</param>
		public Point CellSpotLocation (int x, int y)
		{
			return CellSpotLocation (new Point (x, y));
		}

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		protected override void Draw (GameTime gameTime)
		{
			//var bat = Screen.
			//bat.Begin (SpriteSortMode.BackToFront);
			var bat = Screen.Batch;
			foreach (var x in _objects)
			{
				if (IsVisible (x.Location))
				{
					if (x.Texture == null)
						Console.WriteLine ();
					var rectOutput = new Rectangle (
						                 CellSpotLocation (x.Location),
						                 (Size)CellSize);
					x.Draw (bat, rectOutput);
				}
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

		/// <summary>
		/// Agrega el contenido a la biblitoeca
		/// </summary>
		protected override void AddContent (Moggle.BibliotecaContenido manager)
		{
			base.AddContent (manager);
			foreach (var x in _objects)
				x.AddContent (manager);
		}

		/// <summary>
		/// Vincula el contenido a campos de clase
		/// </summary>
		/// <param name="manager">Manager.</param>
		protected override void InitializeContent (Moggle.BibliotecaContenido manager)
		{
			base.InitializeContent (manager);
			foreach (var x in _objects)
				x.InitializeContent (manager);
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public override void Update (GameTime gameTime)
		{
			TimeManager.ExecuteNext ();
		}

		/// <summary>
		/// Se ejecuta antes del ciclo, pero después de saber un poco sobre los controladores.
		/// No invoca LoadContent por lo que es seguro agregar componentes
		/// </summary>
		public override void Initialize ()
		{
			base.Initialize ();
			foreach (var x in _objects)
				x.Initialize ();
		}

		#region Cámara

		/// <summary>
		/// Centra el campo visible en la dirección de una celda.
		/// </summary>
		/// <param name="p">P.</param>
		public void TryCenterOn (Point p)
		{
			var left = Math.Max (0, p.X - VisibleCells.Width / 2);
			var top = Math.Max (0, p.Y - VisibleCells.Height / 2);
			CurrentVisibleTopLeft = new Point (left, top);
		}

		/// <summary>
		/// Determina si una dirección de celda es visible actualmente.
		/// </summary>
		/// <param name="p">Dirección de celda.</param>
		public bool IsVisible (Point p)
		{
			return GetVisivilityBox ().Contains (p);
		}

		/// <summary>
		/// Gets a rectangle representing the edges (mod grid) of the view
		/// </summary>
		public Rectangle GetVisivilityBox ()
		{
			return new Rectangle (CurrentVisibleTopLeft, VisibleCells);
		}

		/// <summary>
		/// The size of the edge.
		/// Objects outside this area are considered as "centered enough"
		/// </summary>
		static Size _edgeSize = new Size (4, 3);

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
			var edge = GetVisivilityBox ();
			edge.Inflate (-edge_size.Width, -edge_size.Height);
			return edge.Contains (p);
		}

		#endregion

		#region Movimiento

		/// <summary>
		/// Mueve un objeto, considerando colisiones.
		/// </summary>
		/// <returns><c>true</c>, if cell object was moved, <c>false</c> otherwise.</returns>
		/// <param name="objeto">Objeto a mover</param>
		/// <param name="dir">Dirección de movimiento</param>
		public bool MoveCellObject (ICollidableGridObject objeto,
		                            MovementDirectionEnum dir)
		{
			var moveDir = dir.AsDirectionalPoint ();
			var endLoc = objeto.Location + moveDir;

			var destCell = new Cell (this, endLoc);
			if (_collisionSystem.CanFill (objeto, destCell))
			{
				objeto.Location = endLoc;
				return true;
			}
			return false;
		}

		#endregion

		#region Component container

		void IComponentContainerComponent<IGridObject>.AddComponent (IGridObject component)
		{
			_objects.Add (component);
		}

		bool IComponentContainerComponent<IGridObject>.RemoveComponent (IGridObject component)
		{
			return _objects.Remove (component);
		}

		IEnumerable<IGridObject> IComponentContainerComponent<IGridObject>.Components
		{
			get
			{
				return _objects;
			}
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Cells.Grid"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Cells.Grid"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[Grid: _objects={0}, CellSize={1}, CurrentVisibleTopLeft={2}, ControlTopLeft={3}, VisibleCells={4}, GridSize={5}, ControlSize={6}]",
				_objects,
				CellSize,
				CurrentVisibleTopLeft,
				ControlTopLeft,
				VisibleCells,
				GridSize,
				ControlSize);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.Grid"/> class.
		/// </summary>
		/// <param name="xSize">Grid X-size</param>
		/// <param name="ySize">Grid Y-size</param>
		/// <param name="scr">Screen where this grid belongs to</param>
		public Grid (int xSize, int ySize, Moggle.Screens.IScreen scr)
			: base (scr)
		{
			_collisionSystem = new CollisionSystem ();
			GridSize = new Size (xSize, ySize);
			TimeManager = new GameTimeManager (this);
		}
	}
}