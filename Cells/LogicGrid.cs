using System;
using System.Collections.Generic;
using System.Linq;
using AoM;
using Cells.CellObjects;
using Cells.Collision;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Units;
using Moggle.Controles;

namespace Cells
{
	/// <summary>
	/// Representa la parte lógica de un tablero/mapa
	/// </summary>
	public class LogicGrid : IComponent, IUpdate
	{
		readonly HashSet<IGridObject> _objects = new HashSet<IGridObject> ();
		readonly CollisionSystem _collisionSystem;

		readonly Random _r = new Random ();

		#region IComponent implementation

		void IComponent.AddContent ()
		{
			throw new NotImplementedException ();
		}

		void IComponent.InitializeContent ()
		{
			throw new NotImplementedException ();
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
			throw new NotImplementedException ();
		}

		#endregion

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
			while (cell.Objects.Any (z => z is ICollidableGridObject));
			return ret;
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

		/// <summary>
		/// The time manager.
		/// </summary>
		public GameTimeManager TimeManager;

		/// <summary>
		/// Gets the dimentions lenght of the world, in Grid-long
		/// </summary>
		public Size Size { get; }

		/// <summary>
		/// Gets the currently active object
		/// </summary>
		public IUpdateGridObject CurrentObject { get { return TimeManager.Actual; } }

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
		/// Agrega un objeto al grid.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void AddCellObject (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			Objects.Add (obj);
			AddedObject?.Invoke (this, obj);
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
					if (Helper.Geometry.SquaredEucludeanDistance (anchor, cpoint) < Helper.Geometry.SquaredEucludeanDistance (
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
		/// Libera toda suscripción a esta clase, y también invoca <see cref="Dispose"/> a los objetos de este tablero que 
		/// son <see cref="IDisposable"/>
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Cells.LogicGrid"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Cells.LogicGrid"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Cells.LogicGrid"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Cells.LogicGrid"/> was occupying.</remarks>
		public void Dispose ()
		{
			AddedObject = null;
			foreach (var i in _objects.OfType<IDisposable> ())
				i.Dispose ();
		}

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

		/// <summary>
		/// Ocurre al agregar un objeto
		/// </summary>
		public event EventHandler<IGridObject> AddedObject;

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.LogicGrid"/> class.
		/// </summary>
		/// <param name="xSize">X size.</param>
		/// <param name="ySize">Y size.</param>
		public LogicGrid (int xSize, int ySize)
		{
			_collisionSystem = new CollisionSystem ();
			Size = new Size (xSize, ySize);
			TimeManager = new GameTimeManager (this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.LogicGrid"/> class.
		/// </summary>
		/// <param name="mapSize">Map size.</param>
		public LogicGrid (Size mapSize)
		{
			_collisionSystem = new CollisionSystem ();
			Size = mapSize;
			TimeManager = new GameTimeManager (this);
		}
	}
}