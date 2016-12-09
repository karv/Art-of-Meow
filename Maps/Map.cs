using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Debugging;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Units;

namespace Maps
{
	/// <summary>
	/// Representa un conjunto de características.
	/// Provee un constructor de <see cref="Cells.LogicGrid"/>
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
		/// Should add flavor features, like plants on the ground
		/// </summary>
		public bool AddFeatures = true;

		/// <summary>
		/// Generates a <see cref="LogicGrid"/>
		/// </summary>
		/// <param name="enemyExp">La experiencia de cada enemigo en el grid</param>
		public LogicGrid GenerateGrid (float enemyExp)
		{
			var ret = readMapIntoGrid ();
			makeStairs (ret);
			getMapOptions (ret, enemyExp);

			if (AddFeatures)
				AddRandomFlavorFeatures (ret);
			
			return ret;
		}

		void getMapOptions (LogicGrid grid, float mapDiffExp)
		{
			var uFact = new UnidadFactory (grid);
			var enTeam = new TeamManager (Color.Blue);
			// Leer los flags
			while (!dataStream.EndOfStream)
			{
				var cLine = dataStream.ReadLine ();
				var spl = cLine.Split (':');
				switch (spl [0])
				{
					case "Next": // Establecer aquí el valor de NextMap
						Debug.Assert (spl.Length == 2);

						//var posMaps = new List<string> (mapsWithTag (spl [1].Trim ()));
						break;
					
					case "Enemy": // Agregar un enemigo
						Debug.Assert (spl.Length == 2);
						var enemy = uFact.MakeEnemy (spl [1].Trim (), "Warrior", mapDiffExp);
						enemy.Team = enTeam;
						enemy.Location = grid.GetRandomEmptyCell ();

						grid.AddCellObject (enemy);

						break;
					case "":
						// Un fin de línea vacía es nada
						break;
					default:
						Debug.WriteLine (
							string.Format (
								"Entrada de opción desconocida {0} en un mapa {1}",
								spl [0], dataStream), 
							DebugCategories.MapGeneration);
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
		/// <param name = "enemyExp">Experiencia de cada enemigo</param>
		public static LogicGrid GenerateGrid (StreamReader reader, float enemyExp)
		{
			var map = new Map (reader);
			return map.GenerateGrid (enemyExp);
		}

		/// <summary>
		/// Genera un Grid a partir de un reader
		/// </summary>
		/// <param name="mapFile">Nombre de archivo del mapa</param>
		/// <param name = "enemyExp">Experiencia de cada enemigo</param>
		public static LogicGrid GenerateGrid (string mapFile, float enemyExp)
		{
			var map = new Map (mapFile);
			return map.GenerateGrid (enemyExp);
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

		LogicGrid readMapIntoGrid ()
		{
			var sizeX = int.Parse (dataStream.ReadLine ());
			var sizeY = int.Parse (dataStream.ReadLine ());
			var ret = new LogicGrid (sizeX, sizeY);

			var i = 0;
			var mapSize = sizeX * sizeY;
			while (i < mapSize)
			{
				var ix = i % sizeX;
				var iy = i / sizeX;
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