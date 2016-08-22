using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units;
using System;
using System.Collections.Generic;

namespace Cells
{
	public class Grid : SBC
	{
		public List<ICellObject> Objects = new List<ICellObject> ();
		const double probZacate = 0.1;
		readonly Random _r = new Random ();

		public Point CellSize = new Point (24, 24);

		public Point GridSize { get; }

		public Grid (int xSize, int ySize, Moggle.Screens.IScreen scr)
			: base (scr)
		{
			GridSize = new Point (xSize, ySize);
			// Inicializar cada celda
			foreach (var x in contorno ())
			{
				var newObj = new CellObject ("brick-wall", Screen.Content);
				newObj.Depth = Depths.Foreground;
				newObj.CollidePlayer = true;
				newObj.UseColor = Color.DarkGray;
				newObj.Location = x;
				Objects.Add (newObj);
			}

			for (int i = 0; i < xSize; i++)
				for (int j = 0; j < ySize; j++)
				{
					Objects.Add (new BackgroundCellObject (
						new Point (i, j),
						"floor",
						Screen.Content));
					if (_r.NextDouble () < probZacate)
					{
						var newObj = new CellObject ("vanilla-flower", Screen.Content);
						newObj.Location = new Point (i, j);
						newObj.Depth = Depths.GroundDecoration;
						newObj.CollidePlayer = false;
						newObj.UseColor = Color.Green;
						Objects.Add (newObj);
					}
				}				
		}

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

		/// <summary>
		/// Agrega un objeto al grid.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void AddCellObject (ICellObject obj)
		{
			Objects.Add (obj);
		}

		/// <summary>
		/// Celda de _data que se muestra en celda visible (0,0)
		/// </summary>
		public Point CurrentVisibleTopLeft = Point.Zero;

		/// <summary>
		/// Posición top left del control.
		/// </summary>
		public Point ControlTopLeft = Point.Zero;
		public Point VisibleCells = new Point (50, 20);

		public Point ControlSize
		{
			get
			{
				return new Point (VisibleCells.X * CellSize.X,
					VisibleCells.Y * CellSize.Y);
			}
		}

		/// <summary>
		/// Devuelve la posición de un spot de celda (por lo tanto coordenadas absolutas)
		/// </summary>
		/// <param name="p">coordenadas del spot</param>
		public Point CellSpotLocation (Point p)
		{
			return ControlTopLeft + CellSize * (p - CurrentVisibleTopLeft);
		}

		public Rectangle Bounds
		{
			get
			{
				return new Rectangle (ControlTopLeft, ControlSize);
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

		public override void Dibujar (GameTime gameTime)
		{
			var bat = Screen.GetNewBatch ();
			bat.Begin (SpriteSortMode.BackToFront);
			foreach (var x in Objects)
			{
				if (IsVisible (x.Location))
				{
					var rectOutput = new Rectangle (CellSpotLocation (x.Location), CellSize);
					x.Draw (rectOutput, bat);
				}
			}
			bat.End ();
		}

		public override Moggle.Shape.IShape GetBounds ()
		{
			return (Moggle.Shape.Rectangle)Bounds;
		}

		public override void LoadContent ()
		{
			foreach (var x in Objects)
				x.LoadContent ();
		}

		#region Cámara

		/// <summary>
		/// Centra el campo visible en la dirección de una celda.
		/// </summary>
		/// <param name="p">P.</param>
		public void TryCenterOn (Point p)
		{
			var left = Math.Max (0, p.X - VisibleCells.X / 2);
			var top = Math.Max (0, p.Y - VisibleCells.Y / 2);
			CurrentVisibleTopLeft = new Point (left, top);
		}

		/// <summary>
		/// Determina si una dirección de celda es visible actualmente.
		/// </summary>
		/// <param name="p">Dirección de celda.</param>
		public bool IsVisible (Point p)
		{
			return CurrentVisibleTopLeft.X <= p.X &&
			p.X < CurrentVisibleTopLeft.X + VisibleCells.X &&
			CurrentVisibleTopLeft.Y <= p.Y &&
			p.Y < CurrentVisibleTopLeft.Y + VisibleCells.Y;
		}

		public void CenterIfNeeded (ICellObject obj)
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
		public bool MoveCellObject (ICellObject objeto, MovementDirectionEnum dir)
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
	}
}