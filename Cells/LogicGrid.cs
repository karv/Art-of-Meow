using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AoM;
using Cells.CellObjects;
using Cells.Collision;
using Debugging;
using Helper;
using Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Moggle.Controles;
using MonoGame.Extended;
using Units;

namespace Cells
{
	/// <summary>
	/// Representa la parte lógica de un tablero/mapa
	/// </summary>
	public class LogicGrid : IComponent, IUpdate
	{
		#region Debug

		/// <summary>
		/// Throws an exception if the map has flaws
		/// </summary>
		[Conditional ("DEBUG")]
		public void TestGridIntegrity ()
		{
			// Bound the map
			for (int i = 0; i < Size.Width; i++)
			{
				if (!this [i, 0].EnumerateObjects ().OfType<GridWall> ().Any ())
					throw new Exception ();
				if (!this [i, Size.Height - 1].EnumerateObjects ().OfType<GridWall> ().Any ())
					throw new Exception ();
			}

			for (int i = 1; i < Size.Height - 1; i++)
			{
				if (!this [0, i].EnumerateObjects ().OfType<GridWall> ().Any ())
					throw new Exception ();
				if (!this [Size.Width - 1, i].EnumerateObjects ().OfType<GridWall> ().Any ())
					throw new Exception ();
			}
		}

		#endregion

		#region Data & internals

		readonly Cell [,] _cells;
		readonly Random _r = new Random ();
		readonly CollisionSystem _collisionSystem;

		/// <summary>
		/// Gets the dimentions lenght of the world, in Grid-long
		/// </summary>
		public Size Size { get; }

		/// <summary>
		/// Enumerates the cells
		/// </summary>
		public IEnumerable<Cell> EnumerateCells ()
		{
			for (int ix = 0; ix < Size.Width; ix++)
				for (int iy = 0; iy < Size.Height; iy++)
					yield return this [ix, iy];
		}

		#endregion

		#region Accesors

		/// <summary>
		/// Gets the <see cref="Cell"/> at a specified grid-point
		/// </summary>
		/// <param name="ix">x-index</param>
		/// <param name="iy">y-index</param>
		public Cell this [int ix, int iy]
		{
			get
			{
				if (ix >= Size.Width || iy >= Size.Height)
					return Cell.EmptyCell;
				return _cells [ix, iy];
			}
		}

		/// <summary>
		/// Devuelve la celda que el corresponde a una posición dada sus coordenadas
		/// </summary>
		public Cell this [Point p]
		{
			get
			{
				try
				{
					return _cells [p.X, p.Y];
				}
				catch (IndexOutOfRangeException)
				{
					return Cell.EmptyCell;
				}
			}
		}

		/// <summary>
		/// Builds a <see cref="Cell"/> of the current state of a given point
		/// </summary>
		/// <param name="p">Grid-wise point of the cell</param>
		public Cell GetCell (Point p)
		{
			return this [p];
		}

		/// <summary>
		/// Devuelve las coordenadas de un punto vacío en el tablero
		/// </summary>
		/// <returns>The random empty cell's coords.</returns>
		public Point GetRandomEmptyCell ()
		{
			Point ret;
			Cell cell;
			do
			{
				ret = new Point (_r.Next (Size.Width), _r.Next (Size.Height));
				cell = GetCell (ret);
			}
			while (cell.EnumerateObjects ().Any (
				       z => z is ICollidableGridObject ||
				       z is StairsGridObject));
			return ret;
		}

		/// <summary>
		/// Gets the collection of the grid objects
		/// </summary>
		public IEnumerable<IGridObject> Objects
		{
			get
			{
				foreach (var cell in _cells)
					foreach (var gi in cell.EnumerateObjects ().ToArray ())
						yield return gi;
			}
		}

		#endregion

		#region IComponent implementation

		void IComponent.LoadContent (ContentManager manager)
		{
		}

		#endregion

		#region IUpdateable implementation

		/// <summary>
		/// Update the specified gameTime.
		/// </summary>
		public void Update (GameTime gameTime)
		{
			TimeManager.ExecuteNext ();
		}

		#endregion

		#region IGameComponent implementation

		void IGameComponent.Initialize ()
		{
			TestGridIntegrity ();
		}

		#endregion

		#region Cell finder

		/// <summary>
		/// Determina si un punto (en grid) es visible desde otro punto
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="target">Target.</param>
		public bool IsVisibleFrom (Point source, Point target)
		{
			var line = Geometry.EnumerateLine (source, target);
			foreach (var x in line)
			{
				var pCell = GetCell (x);
				if (pCell.BlocksVisibility ())
					return false;
			}
			return true;
		}

		/// <summary>
		/// Gets a random point of a cell inside this grid
		/// </summary>
		public Point RandomPoint ()
		{
			var size = Size;
			return new Point (
				1 + _r.Next (size.Width - 2),
				1 + _r.Next (size.Height - 2));
		}

		/// <summary>
		/// Devuelve el punto más próximo a otro punto dado, que satisface un predicado
		/// </summary>
		/// <param name="selector">Predicado de selección</param>
		/// <param name="anchor">Punto base</param>
		public Point ClosestCellThat (Predicate<Cell> selector, Point anchor)
		{
			var pRet = Point.Zero; // Valor default
			for (int ix = 0; ix < Size.Width; ix++)
				for (int iy = 0; iy < Size.Height; iy++)
				{
					var cpoint = new Point (ix, iy);
					var cell = GetCell (cpoint);
					if (Geometry.SquaredEucludeanDistance (anchor, cpoint) < Geometry.SquaredEucludeanDistance (
						    anchor,
						    pRet) &&
					    selector (cell))
						pRet = cpoint;
				}

			return pRet;
		}

		/// <summary>
		/// Devuelve el enemigo más próximo a una unidad
		/// </summary>
		/// <returns>The closest enemy.</returns>
		/// <param name="unid">Unid.</param>
		public Point GetClosestEnemy (IUnidad unid)
		{
			Predicate<Cell> selector = delegate(Cell obj)
			{
				var unitInCell = obj.GetAliveUnidadHere ();
				return unitInCell != null && !unid.Team.Equals (unitInCell.Team);
			};
			return ClosestCellThat (selector, unid.Location);
		}

		/// <summary>
		/// Gets the closest point of a visible enemy from a given unidad
		/// </summary>
		public Point GetClosestVisibleEnemy (IUnidad unid)
		{
			Predicate<Cell> selector = delegate(Cell obj)
			{
				if (!unid.CanSee (obj.Location))
					return false;
				var unitInCell = obj.GetAliveUnidadHere ();
				return unitInCell != null && !unid.Team.Equals (unitInCell.Team);
			};
			return ClosestCellThat (selector, unid.Location);
		}

		/// <summary>
		/// Enumrates the visible cells that are visible for a given unidad
		/// </summary>
		public IEnumerable<Cell> GetVisibleCells (IUnidad source)
		{
			return source.VisiblePoints ().Select (GetCell);
		}

		/// <summary>
		/// Enumrates the visible unidades that are alive and visible for a given unidad
		/// </summary>
		public IEnumerable<IUnidad> GetVisibleAliveUnidad (IUnidad source)
		{
			foreach (var cell in GetVisibleCells (source))
			{
				var un = cell.GetAliveUnidadHere ();
				if (un != null)
					yield return un;
			}
		}

		/// <summary>
		/// Gets the closest object from a source
		/// </summary>
		public IGridObject GetClosest (IGridObject source, IEnumerable<IGridObject> objects)
		{
			IGridObject ret = null;
			float lastDist = 0;
			foreach (var ob in objects)
			{
				var cDist = Geometry.SquaredEucludeanDistance (source.Location, ob.Location);
				if (ret == null || cDist < lastDist)
				{
					lastDist = cDist;
					ret = ob;
				}
			}
			if (ret == null)
				throw new InvalidOperationException ("Collection empty.");
			return ret;
		}

		#endregion

		#region Time system

		/// <summary>
		/// The time manager.
		/// </summary>
		public GameTimeManager TimeManager;

		/// <summary>
		/// Gets the currently active object
		/// </summary>
		public IUpdateGridObject CurrentObject { get { return TimeManager.Actual; } }

		#endregion

		#region Dynamic cell control

		/// <summary>
		/// Mueve un objeto, considerando colisiones.
		/// </summary>
		/// <returns><c>true</c>, if cell object was moved, <c>false</c> otherwise.</returns>
		/// <param name="objeto">Objeto a mover</param>
		/// <param name="dir">Dirección de movimiento</param>
		public bool MoveCellObject (ICollidableGridObject objeto,
		                            MovementDirectionEnum dir)
		{
			if (dir == MovementDirectionEnum.NoMov)
				return false;

			var moveDir = dir.AsDirectionalPoint ();
			var endLoc = objeto.Location + moveDir;

			var destCell = this [endLoc];
			var moveObj = objeto as IGridMoveable;
			var shouldMove = moveObj?.CanMove (endLoc) ?? true;
			if (shouldMove && _collisionSystem.CanFill (objeto, destCell))
			{
				moveObj?.BeforeMoving (endLoc);
				this [objeto.Location].Remove (objeto);
				objeto.Location = endLoc;
				this [objeto.Location].Add (objeto);
				moveObj?.AfterMoving (endLoc);
				return true;
			}

			var shouldReturnTrue = false;
			// Check if the destination object is activable in case is currently collidable
			foreach (var cell in destCell.EnumerateObjects ().OfType<IActivable> ())
			{
				cell.Activar ();
				var strM = string.Format ("{0} was activated via melee + obstruction", cell);
				Debug.WriteLine (strM, DebugCategories.GridItemsInteraction);
				shouldReturnTrue = true;
			}

			return shouldReturnTrue;
		}

		/// <summary>
		/// Agrega un objeto al grid.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void AddCellObject (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			var cell = this [obj.Location];
			cell.Add (obj);
			ObjectAdded?.Invoke (this, obj);
		}

		/// <summary>
		/// Removes an object from the grid
		/// </summary>
		/// <param name="obj">Object to remove</param>
		public void RemoveObject (IGridObject obj)
		{
			this [obj.Location].Remove (obj);
		}

		#endregion

		#region Memory

		/// <summary>
		/// Libera toda suscripción a esta clase, y también invoca <see cref="Dispose"/> a los objetos de este tablero que 
		/// son <see cref="IDisposable"/>
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Cells.LogicGrid"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Cells.LogicGrid"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Cells.LogicGrid"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Cells.LogicGrid"/> was occupying.</remarks>
		public void Dispose ()
		{
			ObjectAdded = null;
			foreach (var i in Objects.OfType<IDisposable> ())
				i.Dispose ();
		}

		#endregion

		#region World

		/// <summary>
		/// Devuelve el conector de tableros
		/// </summary>
		public GridConnector LocalTopology { get; }

		#endregion

		#region Factory

		/// <summary>
		/// Get or set the enemy generator
		/// </summary>
		public Populator Factory { get; set; }

		#endregion

		#region Events

		/// <summary>
		/// Ocurre al agregar un objeto
		/// </summary>
		public event EventHandler<IGridObject> ObjectAdded;

		#endregion

		#region Ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.LogicGrid"/> class.
		/// </summary>
		/// <param name="xSize">X size.</param>
		/// <param name="ySize">Y size.</param>
		public LogicGrid (int xSize, int ySize)
		{
			_collisionSystem = new CollisionSystem ();
			Size = new Size (xSize, ySize);
			_cells = new Cell[Size.Width, Size.Height];
			for (int ix = 0; ix < Size.Width; ix++)
				for (int iy = 0; iy < Size.Height; iy++)
					_cells [ix, iy] = new Cell (new Point (ix, iy));
			TimeManager = new GameTimeManager (this);
			LocalTopology = new GridConnector (this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.LogicGrid"/> class.
		/// </summary>
		/// <param name="mapSize">Map size.</param>
		public LogicGrid (Size mapSize)
			: this (mapSize.Width, mapSize.Height)
		{
		}
	}

	#endregion
}