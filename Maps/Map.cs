using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AoM;
using Cells;
using Cells.CellObjects;
using Helper;
using Items;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Units;

namespace Maps
{
	/// <summary>
	/// Representa un conjunto de características.
	/// Provee un constructor de <see cref="Cells.LogicGrid"/>
	/// </summary>
	public class Map
	{
		#region Data & internals

		static char [,] generateData ()
		{
			const string jsScriptRandomMapFileName = @"Data/Maps/RandomMapDataScript.js";
			const string functionName = "obtenerJSON";

			var code = File.ReadAllText (jsScriptRandomMapFileName);
			var eng = new Jurassic.ScriptEngine ();
			eng.Evaluate (code);
			var r = eng.CallGlobalFunction (functionName) as string;

			string [] strs = JsonConvert.DeserializeObject<string []> (r);
			var ret = toMatrix (strs);

			// Bound the map
			for (int i = 0; i < ret.GetLength (0); i++)
			{
				ret [i, 0] = 'W';
				ret [i, ret.GetLength (1) - 1] = 'W';
			}

			for (int i = 1; i < ret.GetLength (1) - 1; i++)
			{
				ret [0, i] = 'W';
				ret [ret.GetLength (0) - 1, i] = 'W';
			}

			return ret;
		}

		readonly Random _r;

		/// <summary>
		/// Should add flavor features, like plants on the ground
		/// </summary>
		[JsonProperty (Order = 4)]
		public bool AddFeatures = true;

		/// <summary>
		/// Gets or sets the distribution used to produce items
		/// </summary>
		[JsonProperty (Order = 5)]
		public ProbabilityInstanceSet<IItemFactory> MapItemGroundItems { get; set; }

		#endregion

		#region Generator

		LogicGrid buildBaseGrid (char [,] data)
		{
			var sizeX = data.GetLength (0);
			var sizeY = data.GetLength (1);
			var ret = new LogicGrid (sizeX, sizeY);
			for (int ix = 0; ix < sizeX; ix++)
				for (int iy = 0; iy < sizeY; iy++)
				{
					var newObj = MakeObject (data [ix, iy], ret, new Point (ix, iy));
					ret.AddCellObject (newObj);
				}
			return ret;
		}

		/// <summary>
		/// The populator has the capacity of generate new unts for this map
		/// </summary>
		[JsonProperty (Order = 3)]
		public Populator Populator;

		/// <summary>
		/// Generates a <see cref="LogicGrid"/>
		/// </summary>
		/// <param name="enemyExp">La experiencia de cada enemigo en el grid</param>
		public LogicGrid GenerateGrid (float enemyExp)
		{
			var data = generateData ();
			var ret = buildBaseGrid (data);

			makeStairs (ret);

			ret.Factory = Populator;
			var enemies = Populator.BuildPop (ret, enemyExp);
			populateWith (enemies, ret); 

			if (AddFeatures)
				addRandomFlavorFeatures (ret);

			if (MapItemGroundItems != null)
				addDropItems (ret);

			ret.TestGridIntegrity ();
			return ret;
		}

		static readonly Color enemyColor = Color.Blue;

		static void populateWith (IEnumerable<Unidad> unids, LogicGrid grid)
		{
			var enemyTeam = new TeamManager (enemyColor);
			foreach (var enemy in unids)
			{
				enemy.Team = enemyTeam;

				var point = grid.GetRandomEmptyCell ();
				enemy.Location = point;
				grid [point].Add (enemy);
			}
		}

		void addDropItems (LogicGrid grid)
		{
			var items = MapItemGroundItems.Pick ();
			foreach (var x in items)
			{
				var loc = grid.GetRandomEmptyCell ();
				var newItem = x.Create ();
				var groundItem = new GroundItem (newItem, grid);
				groundItem.Location = loc;
				grid.AddCellObject (groundItem);
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
				throw new ArgumentOutOfRangeException ("p", "Point outsite grid bounds");
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
		void addRandomFlavorFeatures (LogicGrid grid)
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
			var stairDown = new StairsGridObject (grid) { Location = down, UseColor = Color.Green };
			grid.AddCellObject (stairDown);
			grid.LocalTopology.AddConnection (down);
		}

		/// <summary>
		/// Genera un Grid a partir de un reader
		/// </summary>
		/// <param name="mapFile">Nombre de archivo del mapa</param>
		/// <param name = "enemyExp">Experiencia de cada enemigo</param>
		public static LogicGrid GenerateGrid (string mapFile, float enemyExp)
		{
			var map = Map.ReadFromFile (mapFile);
			return map.GenerateGrid (enemyExp);
		}

		#endregion

		#region Symbol language

		static char [] goodSymbols = { ' ', 'W', '\n', '\r' };

		/// <summary>
		/// Determines if a <c>char</c> represents is a map symbol representing an object
		/// </summary>
		public static bool ExistSymbol (char c)
		{
			return goodSymbols.Contains (c);
		}

		#endregion

		#region Ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Map"/> class
		/// </summary>
		/// <param name="size">Size</param>
		public Map (Size size)
		{
			_r = new Random ();
		}

		[JsonConstructor]
		Map (Populator Populator)
		{
			_r = new Random ();

			this.Populator = Populator;
		}

		static char[,] toMatrix (string [] strs)
		{
			try
			{
				var ret = new char[strs [0].Length, strs.Length];
				for (var ix = 0; ix < strs [0].Length; ix++)
					for (var iy = 0; iy < strs.Length; iy++)
						ret [ix, iy] = strs [iy] [ix];
				return ret;
				
			}
			catch (Exception ex)
			{
				throw new Exception ("Error trying to generate a random map", ex);
			}
		}


		#endregion

		#region IO & Json

		/// <summary>
		/// Devuelve un mapa aleatorio del directorio de mapas
		/// </summary>
		public static Map GetRandomMap ()
		{
			var mapDir = new DirectoryInfo (FileNames.MapFolder);
			var maps = mapDir.GetFiles ("*.map.json");
			var _r = new Random ();

			var ret = Map.ReadFromFile (maps [_r.Next (maps.Length)].FullName);
			return ret;
		}

		/// <summary>
		/// The Default settings for json files
		/// </summary>
		public static JsonSerializerSettings JsonSets = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
			NullValueHandling = NullValueHandling.Ignore,
			ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			PreserveReferencesHandling = PreserveReferencesHandling.Objects,
			ObjectCreationHandling = ObjectCreationHandling.Auto,
			MetadataPropertyHandling = MetadataPropertyHandling.Default,
			Formatting = Formatting.Indented,

			Error = onError
		};

		static void onError (object sender,
		                     Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			var err = e.ErrorContext.Error;
			Console.WriteLine (err);
		}

		/// <summary>
		/// Read and return a new <see cref="Map"/> from a json
		/// </summary>
		/// <param name="json">Json-formatted data</param>
		public static Map ReadFromJSON (string json)
		{
			var map = JsonConvert.DeserializeObject<Map> (json, JsonSets);
			return map;
		}

		/// <summary>
		/// Converts the map into a JSON formatted string
		/// </summary>
		public string ToJSON ()
		{
			return JsonConvert.SerializeObject (this, JsonSets);
		}

		/// <summary>
		/// Read and return a new <see cref="Map"/> from a json in a file
		/// </summary>
		/// <param name = "fileName">Nme of the file to read from</param>
		public static Map ReadFromFile (string fileName)
		{
			var file = File.OpenText (fileName);
			var jsonStr = file.ReadToEnd ();
			file.Close ();
			Debug.WriteLine (jsonStr, Debugging.DebugCategories.MapGeneration);
			return Map.ReadFromJSON (jsonStr);
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

		#endregion
	}
}