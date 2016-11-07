using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Moggle.Screens;
using MonoGame.Extended;
using System.Diagnostics;
using Screens;
using Units;

namespace Maps
{
	/// <summary>
	/// Representa un conjunto de características.
	/// Provee un constructor de <see cref="Cells.Grid"/>
	/// </summary>
	public class Map
	{
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

		readonly StreamReader dataStream;

		/// <summary>
		/// El nombre del archivo de mapa del siguiente nivel para ser pasado al <see cref="Grid"/> generado
		/// </summary>
		public string NextMap = @"Maps/base.map";

		/// <summary>
		/// Should add flavor features, like plants on the ground
		/// </summary>
		public bool AddFeatures = true;

		/*
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

		*/
		/// <summary>
		/// Generates a <see cref="Grid"/>
		/// </summary>
		public Grid GenerateGrid (IScreen scr)
		{
			var ret = readMapIntoGrid (scr);
			makeStairs (ret);
			getMapOptions (ret);

			if (AddFeatures)
				AddRandomFlavorFeatures (ret);
			
			ret.DownMap = NextMap; // Establecer el mapa del siguiente nivel
			return ret;
		}

		void getMapOptions (Grid grid)
		{
			// Leer los flags
			while (!dataStream.EndOfStream)
			{
				var cLine = dataStream.ReadLine ();
				var spl = cLine.Split (':');
				var uFact = new UnidadFactory (grid);
				var enTeam = new TeamManager (Color.Blue);
				switch (spl [0])
				{
					case "Next": // Establecer aquí el valor de NextMap
						Debug.Assert (spl.Length == 2);

						var posMaps = new List<string> (mapsWithTag (spl [1].Trim ()));
						NextMap = posMaps [_r.Next (posMaps.Count)];
						break;
					
					case "Enemy": // Agregar un enemigo
						Debug.Assert (spl.Length == 2);
						var enemy = uFact.MakeEnemy (spl [1].Trim ());
						enemy.Team = enTeam;
						enemy.Location = getEmptyCell (grid);
						grid.AddCellObject (enemy);

						break;
					default:
						Debug.WriteLine (
							"Entrada de opción desconocida {0} en un mapa {1}",
							spl [0],
							dataStream);
						break;
				}
			}
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
			if (p.X < 0 || p.Y < 0 || p.X >= grid.GridSize.Width || p.Y >= grid.GridSize.Height)
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
					return null;
			}
			throw new FormatException ("Unknown accepted map symbol " + c);
		}

		/// <summary>
		/// Adds random flavored features to a grid
		/// </summary>
		/// <param name="grid">Grid.</param>
		public void AddRandomFlavorFeatures (Grid grid)
		{
			const float probZacate = 0.1f;
			var mapSize = grid.GridSize;
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

		Point getEmptyCell (Grid grid)
		{
			Point ret;
			var mapSize = grid.GridSize;
			Cell cell;
			do
			{
				ret = new Point (_r.Next (mapSize.Width), _r.Next (mapSize.Height));
				cell = grid.GetCell (ret);
			}
			while (cell.Objects.Count > 1); // REMARK: Siembre hay un gridObject que permite el movimiento
			return ret;
		}


		void makeStairs (Grid grid)
		{
			var down = getEmptyCell (grid);
			var stairDown = new StairsGridObject ("floor", grid)
			{
				UseColor = Color.DarkOrange,
				Depth = Depths.Foreground,
				Location = down
			};
			grid.AddCellObject (stairDown);
		}

		/// <summary>
		/// Genera un Grid a partir de un reader
		/// </summary>
		/// <param name="reader">Un StreamReader con la info del mapa</param>
		/// <param name="scr">Screen del grid</param>
		public static Grid GenerateGrid (StreamReader reader, MapMainScreen scr)
		{
			var map = new Map (reader);
			return map.GenerateGrid (scr);
		}

		/// <summary>
		/// Genera un Grid a partir de un reader
		/// </summary>
		/// <param name="mapFile">Nombre de archivo del mapa</param>
		/// <param name="scr">Screen del grid</param>
		public static Grid GenerateGrid (string mapFile, MapMainScreen scr)
		{
			var map = new Map (mapFile);
			return map.GenerateGrid (scr);
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
			dataStream = reader;
			_r = new Random ();
		}

		Grid readMapIntoGrid (IScreen scr)
		{
			var sizeX = int.Parse (dataStream.ReadLine ());
			var sizeY = int.Parse (dataStream.ReadLine ());
			var ret = new Grid (sizeX, sizeY, scr);

			var i = 0;
			var mapSize = sizeX * sizeY;
			while (i < mapSize)
			{
				var ix = i % sizeY;
				var iy = i / sizeY;
				var chr = (char)dataStream.Read ();
				var obj = MakeObject (chr, ret, new Point (ix, iy));
				if (obj != null)
				{					
					ret.AddCellObject (obj);
					i++;
				}
			}

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