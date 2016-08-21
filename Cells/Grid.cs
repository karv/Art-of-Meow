using Moggle.Controles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Cells.CellObjects;

namespace Cells
{
	public class Grid : SBC
	{
		readonly Cell [,] _data;
		public Point CellSize = new Point (24, 24);

		public Grid (int xSize, int ySize, Moggle.Screens.IScreen scr)
			: base (scr)
		{
			_data = new Cell[xSize, ySize];
			for (int i = 0; i < xSize; i++)
				for (int j = 0; j < ySize; j++)
					_data [i, j] = new Cell ();

			// Inicializar cada celda
			foreach (var x in _data)
				x.AddObject (new BackgroundCellObject ("floor", Screen.Content));

			_data [0, 0].AddObject (new PersonCellObject ("person", Screen.Content));
		}

		/// <summary>
		/// Celda de _data que se muestra en celda visible (0,0)
		/// </summary>
		public Point CurrentVisibleTopLeft = Point.Zero;

		/// <summary>
		/// Posición top left del control.
		/// </summary>
		public Point ControlTopLeft = Point.Zero;
		public Point VisibleCells = new Point (10, 5);

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
			return ControlTopLeft + CellSize * p;
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
			for (int i = 0; i < VisibleCells.X; i++)
				for (int j = 0; j < VisibleCells.Y; j++)
				{
					// La celda que se está dibujando.
					var currCell = _data [CurrentVisibleTopLeft.X + i, 
						               CurrentVisibleTopLeft.Y + j];
					var rectOutput = new Rectangle (CellSpotLocation (i, j), CellSize);
					currCell.Dibujar (Screen.Batch, rectOutput);
				}
			bat.End ();
		}

		public override Moggle.Shape.IShape GetBounds ()
		{
			return (Moggle.Shape.Rectangle)Bounds;
		}

		public override void LoadContent ()
		{
			foreach (var x in _data)
				x.LoadContent ();
		}
	}
}