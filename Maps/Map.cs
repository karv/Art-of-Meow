using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
		char [,] _data;

		readonly Random _r;

		/// <summary>
		/// Sets the data as a string
		/// </summary>
		/// <value>The data.</value>
		[JsonProperty (Order = 2)]
		public IEnumerable<string> Data
		{
			set
			{ 
				MapParser.StringToData (value, _data); 
			}
			get
			{ 
				return MapParser.DataToString (_data);
			}
		}

		/// <summary>
		/// Should add flavor features, like plants on the ground
		/// </summary>
		[JsonProperty (Order = 4)]
		public bool AddFeatures = true;

		/// <summary>
		/// Generates a <see cref="LogicGrid"/>
		/// </summary>
		/// <param name="enemyExp">La experiencia de cada enemigo en el grid</param>
		public LogicGrid GenerateGrid (float enemyExp)
		{
			var ret = buildBaseGrid ();
			makeStairs (ret);

			var factory = new UnidadFactory (ret);
			ret.Factory = new EnemySmartGenerator (factory, enemyExp);

			foreach (var x in EnemyType)
				ret.Factory.AddEnemyType (x);
			ret.Factory.PopulateGrid ();

			if (AddFeatures)
				addRandomFlavorFeatures (ret);

			if (MapItemGroundItems != null)
				addDropItems (ret);
			
			return ret;
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
		/// Gets or sets the distribution used to produce items
		/// </summary>
		[JsonProperty (Order = 4)]
		public ProbabilityInstanceSet<IItemFactory> MapItemGroundItems { get; set; }

		/// <summary>
		/// Gets or sets the type of enemies in this map
		/// </summary>
		/// <value>The type of the enemy.</value>
		[JsonProperty (Order = 3)]
		public IEnumerable<EnemySmartGenerator.EnemyGenerationData> EnemyType { get; set; }

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

		static char [] goodSymbols = { ' ', 'W', '\n', '\r' };

		/// <summary>
		/// Determines if a <c>char</c> represents is a map symbol representing an object
		/// </summary>
		public static bool ExistSymbol (char c)
		{
			return goodSymbols.Contains (c);
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
			var stairDown = new StairsGridObject (grid) { Location = down };
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Map"/> class
		/// </summary>
		/// <param name="size">Size</param>
		public Map (Size size)
		{
			_r = new Random ();
			_data = new char[size.Width, size.Height];
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
			var maps = mapDir.GetFiles ("*.map.json");
			var _r = new Random ();

			var ret = Map.ReadFromFile (maps [_r.Next (maps.Length)].FullName);
			return ret;
		}

		internal static Map HardCreateNew ()
		{
			var ret = new Map (new Size (50, 50));
			for (int i = 0; i < 50 * 50; i++)
			{
				ret._data [i % 50, i / 50] = ' ';
			}
			ret._data [0, 0] = 'W';
			ret.EnemyType = new []
			{
				new EnemySmartGenerator.EnemyGenerationData (
					Units.EnemyType.Imp,
					EnemyClass.Warrior)
			};
			return ret;
		}

		/// <summary>
		/// Gets the horizontal size
		/// </summary>
		[JsonProperty (Order = 1)]
		public int SizeX { get { return _data.GetLength (0); } }

		/// <summary>
		/// Gets the vertical size
		/// </summary>
		[JsonProperty (Order = 0)]
		public int SizeY { get { return _data.GetLength (1); } }

		/// <summary>
		/// Gets the size of this map
		/// </summary>
		[JsonIgnore]
		public Size Size
		{ 
			get { return new Size (SizeX, SizeY); } 
		}

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

		IEnumerable <string> unparseMapObjects ()
		{
			var line = new StringBuilder ();
			for (int ix = 0; ix < SizeX; ix++)
			{
				for (int iy = 0; iy < SizeY; iy++)
					line.Append (_data [ix, iy]);
				yield return line.ToString ();
				line.Clear ();
			}		
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

		LogicGrid buildBaseGrid ()
		{
			var ret = new LogicGrid (Size);
			for (int ix = 0; ix < SizeX; ix++)
				for (int iy = 0; iy < SizeY; iy++)
				{
					var newObj = MakeObject (_data [ix, iy], ret, new Point (ix, iy));
					ret.AddCellObject (newObj);
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

		[JsonConstructor]
		Map (int sizeX, int sizeY,
		     IEnumerable<string> data,
		     IEnumerable<EnemySmartGenerator.EnemyGenerationData> enemyType)
			: this (new Size (sizeX, sizeY))
		{
			Data = data;
			EnemyType = enemyType;
		}
	}
}