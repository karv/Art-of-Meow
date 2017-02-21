using Newtonsoft.Json;

namespace AoM
{
	public class DamageAttributesManager : IdentificableManager<DamageAttribute>
	{
		public DamageAttribute this [string name]
		{
			get { return Get (name); }
		}

		[JsonConstructor]
		public DamageAttributesManager (DamageAttribute [] Collection)
			: base (Collection)
		{
		}

		readonly static JsonSerializerSettings Sets = new JsonSerializerSettings
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


		public static DamageAttributesManager FromFile ()
		{
			return FromFile<DamageAttributesManager> (FileNames.DamageAttributes, Sets);
		}
	}
}