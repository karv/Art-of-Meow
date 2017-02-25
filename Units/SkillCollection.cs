using System.IO;
using AoM;
using Newtonsoft.Json;
using Units.Skills;

namespace Units
{
	/// <summary>
	/// Manager and database of all defined skills
	/// </summary>
	public class SkillCollection : IdentificableManager<ISkill>
	{
		/// <summary>
		/// Gets the skill with a specified name.
		/// </summary>
		public ISkill GetSkill (string name)
		{
			return Get (name);
		}

		/// <summary>
		/// The json settings
		/// </summary>
		public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto,
			TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
			NullValueHandling = NullValueHandling.Ignore,
			ReferenceLoopHandling = ReferenceLoopHandling.Error,
			PreserveReferencesHandling = PreserveReferencesHandling.None,
			ObjectCreationHandling = ObjectCreationHandling.Auto,
			MetadataPropertyHandling = MetadataPropertyHandling.Default,
			Formatting = Formatting.Indented
		};

		/// <summary>
		/// </summary>
		[JsonConstructor]
		public SkillCollection (ISkill [] Skills)
			: base (Skills)
		{
		}

		/// <summary>
		/// Gets the <see cref="UnitClassRaceManager"/> from a file
		/// </summary>
		public static SkillCollection FromFile (string fileName = FileNames.Skills)
		{
			var file = File.OpenText (fileName);
			var jsonStr = file.ReadToEnd ();
			file.Close ();
			return JsonConvert.DeserializeObject<SkillCollection> (jsonStr, JsonSettings);
		}
	}
}