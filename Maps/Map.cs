using MonoGame.Extended;
using Cells;
using Moggle.Screens;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.ES20;

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
		readonly Random _r;

		public bool BoundGrid = true;
		public bool AddFeatures = true;

		public void AddBoundsTo (char c, Grid grid)
		{
			foreach (var pt in contorno())
				grid.AddCellObject (MakeObject (c, grid, pt));
		}

		/// <summary>
		/// enumera las celdas de contorno.
		/// </summary>
		IEnumerable<Point> contorno ()
		{
			int sizeX = MapSize.Width;
			int sizeY = MapSize.Height;
			for (int i = 0; i < sizeX; i++)
			{
				yield return (new Point (i, 0));
				yield return (new Point (i, sizeY - 1));
			}
			for (int i = 1; i < sizeY - 1; i++)
			{
				yield return (new Point (0, i));
				yield return (new Point (sizeX - 1, i));
			}
		}


		/// <summary>
		/// Generates a <see cref="Grid"/>
		/// </summary>
		public Grid GenerateGrid (IScreen scr)
		{
			var ret = new Grid (MapSize.Width, MapSize.Height, scr);

			for (int ix = 0; ix < MapSize.Width; ix++)
				for (int iy = 0; iy < MapSize.Height; iy++)
					ret.AddCellObject (MakeObject (_data [ix, iy], ret, new Point (ix, iy)));

			if (BoundGrid)
				AddBoundsTo ('W', ret);
			if (AddFeatures)
				AddRandomFlavorFeatures (ret);
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
				case (char)0:
					return new BackgroundObject (p, "floor", grid);
				case 'W':
					var newObj = new GridObject ("brick-wall", grid);
					newObj.Depth = Depths.Foreground;
					newObj.CollidePlayer = true;
					newObj.UseColor = Color.DarkGray;
					newObj.Location = p;
					return newObj;
			}
			throw new Exception ("Unknown map symbol " + c);
		}

		public void AddRandomFlavorFeatures (Grid grid)
		{
			const float probZacate = 0.1f;
			for (int ix = 0; ix < MapSize.Width; ix++)
				for (int iy = 0; iy < MapSize.Height; iy++)
					if (_r.NextDouble () < probZacate)
					{
						var newObj = new GridObject ("vanilla-flower", grid);
						newObj.Location = new Point (ix, iy);
						newObj.Depth = Depths.GroundDecoration;
						newObj.CollidePlayer = false;
						newObj.UseColor = Color.Green;
						grid.AddCellObject (newObj);
					}
		}

		public Map (char [,] data)
		{
			_r = new Random ();
			_data = new char[data.GetLength (0), data.GetLength (1)];
			data.CopyTo (_data, 0);
		}

		public Map ()
		{
			_r = new Random ();
			_data = new char[0, 0];
		}

		public Map (Size size)
		{
			_r = new Random ();
			_data = new char[size.Width, size.Height];
		}

		public Map (string fileName)
			: this (new StreamReader (fileName))
		{
		}

		public Map (StreamReader reader)
		{
			var sizeX = int.Parse (reader.ReadLine ());
			var sizeY = int.Parse (reader.ReadLine ());
			_data = new char[sizeX, sizeY];
			_r = new Random ();

			for (int i = 0; i < sizeY; i++)
			{
				var currLine = new char[sizeX];
				reader.ReadBlock (currLine, 0, sizeX);

				for (int j = 0; j < sizeX; j++)
					_data [j, i] = currLine [j];
			}
		}
	}
}