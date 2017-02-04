using System.IO;
using System.Linq;
using AoM;
using Newtonsoft.Json;

namespace Items
{
	/// <summary>
	/// Manages the collection of all the items by maintaining them.
	/// </summary>
	public class ItemDatabase
	{
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
		/// Collection of items
		/// </summary>
		public readonly IItem [] Collection;

		/// <summary>
		/// Creates and returns an specified item.
		/// </summary>
		/// <param name="itemId">The index of the item</param>
		public T CreateItem<T> (int itemId) 
			where T : IItem
		{
			return (T)Collection [itemId].Clone ();
		}

		/// <summary>
		/// Creates and returns an specified item.
		/// </summary>
		/// <param name="itemName">Base name of the item</param>
		public T CreateItem<T> (string itemName) 
			where T : IItem
		{
			// REMARK: OfType is used to remove redundances as much as possible
			return (T)Collection.OfType<T> ().First (z => z.NombreBase == itemName).Clone ();
		}

		/// <summary>
		/// Creates and returns an specified item.
		/// </summary>
		/// <param name="itemId">The index of the item</param>
		public IItem CreateItem (int itemId)
		{
			return Collection [itemId].Clone () as IItem;
		}

		/// <summary>
		/// Creates and returns an specified item.
		/// </summary>
		/// <param name="itemName">The name of the item</param>
		public IItem CreateItem (string itemName)
		{
			return Collection.First (z => z.NombreBase == itemName).Clone () as IItem;
		}

		/// <summary>
		/// Constructs a new <see cref="ItemDatabase"/> from a json file
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileName">File name.</param>
		public static ItemDatabase FromFile (string fileName = FileNames.Items)
		{
			var file = File.OpenText (fileName);
			var jsonStr = file.ReadToEnd ();
			file.Close ();

			var ret = JsonConvert.DeserializeObject<ItemDatabase> (jsonStr, JsonSettings);

			return ret;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.ItemDatabase"/> class.
		/// </summary>
		[JsonConstructor]
		public ItemDatabase (IItem [] Collection)
		{
			this.Collection = Collection;
		}
	}
}