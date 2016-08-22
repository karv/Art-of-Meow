using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units;
using System;

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
		}

		/// <summary>
		/// Agrega un objeto a una celda en las coordenadas dadas.
		/// </summary>
		/// <param name="loc">Coordenadas</param>
		/// <param name="obj">Object.</param>
		public void AddCellObject (Point loc, ICellObject obj)
		{
			_data [loc.X, loc.Y].AddObject (obj);
		}

		/// <summary>
		/// Agrega un objeto a una celda en las coordenadas dadas.
		/// </summary>
		/// <param name="loc">Coordenadas</param>
		/// <param name="obj">Objeto localizado</param>
		public void AddCellObject (Point loc, ICellLocalizable obj)
		{
			AddCellObject (loc, obj.CellObject);
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

		/// <summary>
		/// Busca la dirección absoluta de la celda que contiene un objeto dado.
		/// </summary>
		/// <returns>Si no existe, devuelve (-1, -1)</returns>
		/// <param name="obj">Object.</param>
		public bool FindCellAddrWithObject (ICellObject obj, out Point addr)
		{
			for (int i = 0; i < _data.GetLength (0); i++)
				for (int j = 0; j < _data.GetLength (1); j++)
					if (_data [i, j].Contains (obj))
					{
						addr = new Point (i, j);
						return true;
					}
			addr = Point.Zero;
			return false;
		}

		public Cell FindCellWithObject (ICellObject obj)
		{
			Point addr;
			return FindCellAddrWithObject (obj, out addr) ? _data [addr.X, addr.Y] : null;
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
			Point p;
			if (FindCellAddrWithObject (obj, out p))
			{
				if (!IsVisible (p))
					TryCenterOn (p);
			}
			else
				throw new Exception ("Cell object not found");
		}

		#endregion

		#region Movimiento

		public bool MoveCellObject (ICellObject objeto, MovementDirectionEnum dir)
		{
			Point addrFrom;
			if (!FindCellAddrWithObject (objeto, out addrFrom))
				throw new Exception ("No existe objeto que se requiere mover.");
			Point addrTo = Point.Zero;

			switch (dir)
			{
				case MovementDirectionEnum.Down:
					addrTo = new Point (addrFrom.X, addrFrom.Y + 1);
					break;
				default:
					break;
			}

			_data [addrFrom.X, addrFrom.Y].MoveObjectToCell (
				objeto,
				_data [addrTo.X, addrTo.Y]);

			return true;
		}

		#endregion
	}
}