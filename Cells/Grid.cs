using System;
using System.Collections.Generic;
using System.Linq;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units;
using MonoGame.Extended.Shapes;
using MonoGame.Extended;
using Art_of_Meow;

namespace Cells
{
	public class Grid : DSBC, IComponentContainerComponent<IGridObject>
	{

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
		public Vector2 ControlTopLeft = Vector2.Zero;
		public Size VisibleCells = new Size (50, 20);

		public SizeF ControlSize
		{
			get
			{
				return new SizeF (VisibleCells.Width * CellSize.Width,
					VisibleCells.Height * CellSize.Height);
			}
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="p">coordenadas del spot</param>
		public Vector2 CellSpotLocation (Point p)
		{
			var _x = ControlTopLeft.X + CellSize.Width * (p.X - CurrentVisibleTopLeft.X);
			var _y = ControlTopLeft.Y + CellSize.Height * (p.Y - CurrentVisibleTopLeft.Y);
			return new Vector2 (_x, _y);
		}

		public RectangleF Bounds
		{
			get
			{
				return new RectangleF (ControlTopLeft, ControlSize);
			}
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="x">Coordenada X</param>
		/// <param name="y">Coordenada Y</param>
		public Vector2 CellSpotLocation (int x, int y)
		{
			return CellSpotLocation (new Point (x, y));
		}

		public override void Draw (GameTime gameTime)
		{
			//var bat = Screen.
			//bat.Begin (SpriteSortMode.BackToFront);
			var bat = Screen.Batch;
			foreach (var x in new HashSet<IGridObject>  (_objects))
			{
				if (IsVisible (x.Location))
				{
					if (x.Texture == null)
						Console.WriteLine ();
					var rectOutput = new RectangleF (CellSpotLocation (x.Location), CellSize);
					x.Draw (rectOutput, bat);
				}
			}
			bat.End ();
		}

		public override IShapeF GetBounds ()
		{
			return Bounds;
		}

		protected override void LoadContent ()
		{
			foreach (var x in _objects)
				x.LoadContent (Screen.Content);
		}

		public override void Update (GameTime gameTime)
		{
			// applyDelta ();
			TimeManager.ExecuteNext ();
		}

		void updateUnits (GameTime gameTime)
		{
			foreach (var x in Objects.OfType<IUnidad> ())
				x.Update (gameTime);
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
			return CurrentVisibleTopLeft.X <= p.X &&
			p.X < CurrentVisibleTopLeft.X + VisibleCells.Width &&
			CurrentVisibleTopLeft.Y <= p.Y &&
			p.Y < CurrentVisibleTopLeft.Y + VisibleCells.Height;
		}

		public void CenterIfNeeded (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			if (!IsVisible (obj.Location))
				TryCenterOn (obj.Location);
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
					_objects.Add (new BackgroundObject (
						new Point (i, j),
						"floor"));
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