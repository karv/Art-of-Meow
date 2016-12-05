using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Maps
{
	/// <summary>
	/// Representa un conjunto de características.
	/// Provee un constructor de <see cref="Cells.LogicGrid"/>
	/// </summary>
	public class Map
	{
		readonly char [,] _data;
		/*
		/// <summary>
		/// Gets the size of the map.
		/// </summary>
		/// <value>The size of the map.</value>

		public Size MapSize { get { return new Size (
				_data.GetLength (0),
				_data.GetLength (1)); } }
		*/
		readonly Random _r;

		/// <summary>
		/// Sets the data as a string
		/// </summary>
		/// <value>The data.</value>
		public string Data
		{
			set { parseMapObjects (value); }
		}

		/// <summary>
		/// Should add flavor features, like plants on the ground
		/// </summary>
		public bool AddFeatures = true;

		/// <summary>
		/// Generates a <see cref="LogicGrid"/>
		/// </summary>
		public LogicGrid GenerateGrid ()
		{
			var ret = buildBaseGrid ();
			makeStairs (ret);

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
		public IGridObject MakeObject (char c,
		                               LogicGrid grid,
		                               Point p)
		{
			if (grid == null)
				throw new ArgumentNullException ("grid");
			if (p.X < 0 || p.Y < 0 || p.X >= grid.Size.Width || p.Y >= grid.Size.Height)
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
				case '\n':
				case '\r':
					return null;
			}
			throw new FormatException ("Unknown accepted map symbol " + c);
		}

		static char [] goodSymbols = { ' ', 'W', '\n', '\r' };

		public static bool ExistSymbol (char c)
		{
			return goodSymbols.Contains (c);
		}

		/// <summary>
		/// Adds random flavored features to a grid
		/// </summary>
		/// <param name="grid">Grid.</param>
		public void AddRandomFlavorFeatures (LogicGrid grid)
		{
			const float probZacate = 0.1f;
			var mapSize = grid.Size;
			for (int ix = 0; ix < mapSize.Width; ix++)
				for (int iy = 0; iy < mapSize.Height; iy++)
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

		static void makeStairs (LogicGrid grid)
		{
			var down = grid.GetRandomEmptyCell ();
			var stairDown = new StairsGridObject (grid)
			{
				Location = down
			};
			grid.AddCellObject (stairDown);
			grid.LocalTopology.AddConnection (down);
		}

		/// <summary>
		/// Genera un Grid a partir de un reader
		/// </summary>
		/// <param name="reader">Un StreamReader con la info del mapa</param>
		public static LogicGrid GenerateGrid (StreamReader reader)
		{
			var map = new Map (reader);
			return map.GenerateGrid ();
		}

		/// <summary>
		/// Genera un Grid a partir de un reader
		/// </summary>
		/// <param name="mapFile">Nombre de archivo del mapa</param>
		public static LogicGrid GenerateGrid (string mapFile)
		{
			var map = new Map (mapFile);
			return map.GenerateGrid ();
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
			if (reader == null)
				throw new ArgumentNullException ("reader");
			throw new NotImplementedException ("Load Json");
			//dataStream = reader;
			//_r = new Random ();
		}

		/// <summary>
		/// El directorio de los mapas
		/// </summary>
		public const string MapDir = "Maps";

		/// <summary>
		/// Devuelve un mapa aleatorio del directorio de mapas
		/// </summary>
		public static Map GetRandomMap ()
		{
			var mapDir = new DirectoryInfo (MapDir);
			var maps = mapDir.GetFiles ("dung*.map");
			var _r = new Random ();

			var ret = new Map (maps [_r.Next (maps.Length)].FullName);
			return ret;
		}

		public int SizeX { get { return _data.GetLength (0); } }

		public int SizeY { get { return _data.GetLength (1); } }

		public Size Size { get { return new Size (SizeX, SizeY); } }

		void parseMapObjects (string data)
		{
			var i = 0;
			var j = 0;
			var mapSize = SizeX * SizeY;
			while (i < mapSize)
			{
				var ix = i % SizeX;
				var iy = i / SizeX;
				var chr = data [j];
				//var obj = MakeObject (chr, ret, new Point (ix, iy));
				if (ExistSymbol (chr))
				{
					_data [ix, iy] = data [j++];
					i++;
				}
			}
		}

		LogicGrid buildBaseGrid ()
		{
			var ret = new LogicGrid (Size);
			for (int ix = 0; ix < SizeX; ix++)
				for (int iy = 0; iy < SizeY; iy++)
					ret.AddCellObject (MakeObject (_data [ix, iy], ret, new Point (ix, iy)));

			return ret;
		}

		/// <summary>
		/// Enumera los nombres de los archivos de mapa que tengan el TAG dado
		/// </summary>
		static IEnumerable<string> mapsWithTag (string tag)
		{
			var dir = new DirectoryInfo (@"Maps");
			return dir.EnumerateFiles ("*.map").Where (z => mapFileHasTag (z, tag)).Select (z => z.FullName);
		}

		static bool mapFileHasTag (FileInfo file, string Tag)
		{
			var reader = file.OpenText ();
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine ();
				var spl = line.Split (':');
				if (spl.Length > 0 && spl [0].Trim () == "Tag")
				{
					
					Debug.Assert (spl.Length == 2);
					if (spl [1].Trim () == Tag)
						return true;
				}
			}
			return false;
		}
	}
}