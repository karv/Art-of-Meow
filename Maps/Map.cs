using System;
using System.Collections.Generic;
using System.IO;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Moggle.Screens;
using MonoGame.Extended;

namespace Maps
{
	/// <summary>
	/// Representa un conjunto de características.
	/// Provee un constructor de <see cref="Cells.Grid"/>
	/// </summary>
	public class Map
	{
		/// <summary>
		/// Gets the size of the map.
		/// </summary>
		/// <value>The size of the map.</value>
		public Size MapSize { get { return new Size (
				_data.GetLength (0),
				_data.GetLength (1)); } }

		readonly char [,] _data;
		readonly Random _r;

		/// <summary>
		/// Should a bounding rectangle of impassable terrain be added around the map
		/// </summary>
		public bool BoundGrid = true;

		/// <summary>
		/// Should add flavor features, like plants on the ground
		/// </summary>
		public bool AddFeatures = true;

		/// <summary>
		/// Add bounds to a grid, with a tile represented by a character
		/// </summary>
		/// <param name="c">Character representing a <see cref="IGridObject"/></param>
		/// <param name="grid">The grid where the bounds are suposed to be added</param>
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

		/// <summary>
		/// Makes a object from a given <c>char</c>
		/// </summary>
		/// <returns>A <see cref="IGridObject"/> represented by a <c>char</c> value</returns>
		/// <param name="c">A <c>char</c> value representing the <see cref="IGridObject"/></param>
		/// <param name="grid">Grid.</param>
		/// <param name="p">Location grid-wise of the item to add</param>
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
					var newObj = new GridWall ("brick-wall", grid);
					newObj.Location = p;
					return newObj;
			}
			throw new Exception ("Unknown map symbol " + c);
		}

		/// <summary>
		/// Adds random flavored features to a grid
		/// </summary>
		/// <param name="grid">Grid.</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Map"/> class.
		/// </summary>
		/// <param name="data">A 2-dimentional array containing the <c>char</c> value info of every cell in the grid</param>
		public Map (char [,] data)
		{
			_r = new Random ();
			_data = new char[data.GetLength (0), data.GetLength (1)];
			data.CopyTo (_data, 0);
		}

		/// <summary>
		/// Initializes a new void instance of the <see cref="Maps.Map"/> class. 
		/// This constructor will generate a 0x0 map, and this value cannot be changed.
		/// </summary>
		public Map ()
		{
			_r = new Random ();
			_data = new char[0, 0];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Map"/> class of a given size
		/// </summary>
		/// <param name="size">Size of the map in cells-long</param>
		public Map (Size size)
		{
			_r = new Random ();
			_data = new char[size.Width, size.Height];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Map"/> class by reading the content from a .map file
		/// </summary>
		/// <param name="fileName">The .map file name</param>
		public Map (string fileName)
			: this (new StreamReader (fileName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Map"/> class by reading the content from a 
		/// <see cref="StreamReader"/> with a .map format
		/// </summary>
		/// <param name="reader">A stream reader</param>
		public Map (StreamReader reader)
		{
			try
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
			catch (Exception ex)
			{
				throw new IOException ("No se puede cargar archivo de mapa.", ex);
			}
		}
	}
}