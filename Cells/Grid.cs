using System;
using System.Collections.Generic;
using AoM;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Units;

namespace Cells
{
	public class Grid : DSBC, IComponentContainerComponent<IGridObject>
	{
		public Cell GetCell (Point p)
		{
			return new Cell (this, p);
		}

		public ICollection<IGridObject> Objects
		{
			get
			{
				return _objects;
			}
		}

		readonly HashSet<IGridObject> _objects = new HashSet<IGridObject> ();

		const double probZacate = 0.1;
		readonly Random _r = new Random ();

		public SizeF CellSize = new SizeF (24, 24);

		public GameTimeManager TimeManager;

		public Point GridSize { get; }

		public IUpdateGridObject ObjectoActual { get { return TimeManager.Actual; } }

		/// <summary>
		/// enumera las celdas de contorno.
		/// </summary>
		IEnumerable<Point> contorno ()
		{
			for (int i = 0; i < GridSize.X; i++)
			{
				yield return (new Point (i, 0));
				yield return (new Point (i, GridSize.Y - 1));
			}
			for (int i = 1; i < GridSize.Y - 1; i++)
			{
				yield return (new Point (0, i));
				yield return (new Point (GridSize.X - 1, i));
			}
		}

		public Point RandomPoint ()
		{
			var size = GridSize;
			return new Point (1 + _r.Next (size.X - 2), 1 + _r.Next (size.Y - 2));
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
		public Size VisibleCells = new Size (50, 20);

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

		public override void Draw (GameTime gameTime)
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
			bat.End ();
		}

		public override IShapeF GetBounds ()
		{
			return Bounds;
		}

		protected override void LoadContent (ContentManager manager)
		{
			foreach (var x in _objects)
				x.LoadContent (manager);
		}

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

		static Size _edgeSize = new Size (4, 3);

		public void CenterIfNeeded (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");

			if (!IsInCenter (obj.Location, _edgeSize))
				TryCenterOn (obj.Location);
		}

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
		/// <param name="objeto">Objeto</param>
		/// <param name="dir">Dirección</param>
		public bool MoveCellObject (IGridObject objeto, MovementDirectionEnum dir)
		{
			var moveDir = dir.AsDirectionalPoint ();
			var endLoc = objeto.Location + moveDir;

			var destCell = new Cell (this, endLoc);
			if (!destCell.Collision (objeto))
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

		public Grid (int xSize, int ySize, Moggle.Screens.IScreen scr)
			: base (scr)
		{
			GridSize = new Point (xSize, ySize);
			TimeManager = new GameTimeManager (this);
			// Inicializar cada celda
			foreach (var x in contorno ())
			{
				var newObj = new GridObject ("brick-wall");
				newObj.Depth = Depths.Foreground;
				newObj.CollidePlayer = true;
				newObj.UseColor = Color.DarkGray;
				newObj.Location = x;
				_objects.Add (newObj);
			}

			for (int i = 0; i < xSize; i++)
				for (int j = 0; j < ySize; j++)
				{
					AddCellObject (new BackgroundObject (
						new Point (i, j),
						"floor",
						this));
					if (_r.NextDouble () < probZacate)
					{
						var newObj = new GridObject ("vanilla-flower");
						newObj.Location = new Point (i, j);
						newObj.Depth = Depths.GroundDecoration;
						newObj.CollidePlayer = false;
						newObj.UseColor = Color.Green;
						_objects.Add (newObj);
					}
				}
		}
	}
}