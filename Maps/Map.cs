using MonoGame.Extended;
using Cells;
using Moggle.Screens;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using System;

namespace Maps
{
	/// <summary>
	/// Representa un conjunto de características.
	/// Provee un constructor de <see cref="Cells.Grid"/>
	/// </summary>
	public class Map
	{
		public Size MapSize { get { return new Size (
				_data.GetLength (0),
				_data.GetLength (1)); } }

		readonly char [,] _data;

		/// <summary>
		/// Generates a <see cref="Grid"/>
		/// </summary>
		public Grid GenerateGrid (IScreen scr)
		{
			var ret = new Grid (MapSize.Width, MapSize.Height, scr);

			for (int ix = 0; ix < MapSize.Width; ix++)
				for (int iy = 0; iy < MapSize.Height; iy++)
					ret.AddCellObject (MakeObject (_data [ix, iy], ret, new Point (ix, iy)));

			return ret;
		}

		public IGridObject MakeObject (char c, Grid grid, Point p)
		{
			if (grid == null)
				throw new ArgumentNullException ("grid");
			if (p.X < 0 || p.Y < 0 || p.X >= grid.GridSize.X || p.Y >= grid.GridSize.Y)
				throw new Exception ("Point outsite grid bounds");
			switch (c)
			{
				case ' ':
					return new BackgroundObject (p, "floor", grid);
				case 'W':
					var newObj = new GridObject ("brick-wall");
					newObj.Depth = Depths.Foreground;
					newObj.CollidePlayer = true;
					newObj.UseColor = Color.DarkGray;
					newObj.Location = p;
					return newObj;
			}
			throw new Exception ("Unknown map symbol " + c);
		}

		public Map (char [,] data)
		{
			_data = new char[data.GetLength (0), data.GetLength (1)];
			data.CopyTo (_data, 0);
		}

		public Map ()
		{
			_data = new char[0, 0];
		}

		public Map (Size size)
		{
			_data = new char[size.Width, size.Height];
		}

		public Map (string fileName)
		{
			
		}

	}
}