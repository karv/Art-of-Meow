using Newtonsoft.Json;

namespace AoM
{
	/// <summary>
	/// The database and getter of attributes
	/// </summary>
	public class DamageAttributesManager : IdentificableManager<DamageAttribute>
	{
		/// <summary>
		/// Gets the attribute with a given name
		/// </summary>
		public DamageAttribute this [string name]
		{
			get { return Get (name); }
		}

		[JsonConstructor]
		DamageAttributesManager (DamageAttribute [] Collection)
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

		/// <summary>
		/// Loads the data from the file
		/// </summary>
		/// <seealso cref="FileNames"/>
		public static DamageAttributesManager FromFile ()
		{
			return FromFile<DamageAttributesManager> (FileNames.DamageAttributes, Sets);
		}
	}
}