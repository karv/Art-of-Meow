using System.Diagnostics;
using System.IO;
using System.Linq;
using AoM;
using Newtonsoft.Json;

namespace Units
{
	/// <summary>
	/// Manages the collection of classes and races
	/// </summary>
	public class UnitClassRaceManager
	{
		/// <summary>
		/// The classes
		/// </summary>
		public readonly UnitClass [] Class;
		/// <summary>
		/// The races
		/// </summary>
		public readonly UnitRace [] Race;

		/// <summary>
		/// Gets the class
		/// </summary>
		/// <param name="className">The name of the class</param>
		public UnitClass GetClass (string className)
		{
			return Class.First (z => z.Name == className);
		}

		/// <summary>
		/// Gets the race
		/// </summary>
		/// <param name="raceName">The name of the race</param>
		public UnitRace GetRace (string raceName)
		{
			return Race.First (z => z.Name == raceName);
		}

		/// <summary>
		/// The Default settings for json files
		/// </summary>
		[JsonIgnore]
		public static JsonSerializerSettings JsonSets = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
			NullValueHandling = NullValueHandling.Ignore,
			ReferenceLoopHandling = ReferenceLoopHandling.Error,
			PreserveReferencesHandling = PreserveReferencesHandling.None,
			ObjectCreationHandling = ObjectCreationHandling.Auto,
			MetadataPropertyHandling = MetadataPropertyHandling.Default,
			Formatting = Formatting.Indented,

			Error = onError
		};

		static void onError (object sender,
		                     Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			var err = e.ErrorContext.Error;
			Debug.WriteLine (err);
			Debugger.Break ();
		}

		/// <summary>
		/// Gets the <see cref="UnitClassRaceManager"/> from a file
		/// </summary>
		public static UnitClassRaceManager FromFile (string fileName = FileNames.RaceClass)
		{
			var file = File.OpenText (fileName);
			var jsonStr = file.ReadToEnd ();
			file.Close ();
			return JsonConvert.DeserializeObject<UnitClassRaceManager> (jsonStr, JsonSets);
		}

		/// <summary>
		/// </summary>
		[JsonConstructor]
		public UnitClassRaceManager (UnitClass [] Class, UnitRace [] Race)
		{
			this.Class = Class;
			this.Race = Race;
		}
	}
}