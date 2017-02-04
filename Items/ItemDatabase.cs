using System.IO;
using AoM;
using Newtonsoft.Json;

namespace Items
{
	public class ItemDatabase
	{
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

		public readonly IItem [] Collection;

		public static ItemDatabase FromFile (string fileName = FileNames.Items)
		{
			var file = File.OpenText (fileName);
			var jsonStr = file.ReadToEnd ();
			file.Close ();

			var ret = JsonConvert.DeserializeObject<ItemDatabase> (jsonStr, JsonSettings);

			return ret;
		}

		[JsonConstructor]
		public ItemDatabase (IItem [] Collection)
		{
			this.Collection = Collection;
		}
	}
}