using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AoM
{
	/// <summary>
	/// Represents a common mplementation of the object collection of a kind
	/// </summary>
	public abstract class IdentificableManager<T>
		where T : IIdentificable
	{
		/// <summary>
		/// The collection of items
		/// </summary>
		protected readonly T [] Collection;

		/// <summary>
		/// Creates and returns an specified item.
		/// </summary>
		/// <param name="name">Base name of the item</param>
		protected S Get<S> (string name)
			where S : T
		{
			// REMARK: OfType is used to remove redundances as much as possible
			return Collection.OfType<S> ().First (z => z.Name == name);
		}

		/// <summary>
		/// Gets the item with a specified name
		/// </summary>
		protected T Get (string name)
		{
			return Collection.First (z => z.Name == name);
		}

		/// <summary>
		/// </summary>
		/// <param name="Collection">The collection of items</param>
		protected IdentificableManager (T [] Collection)
		{
			this.Collection = Collection;
		}


		/// <summary>
		/// Constructs a new database from a json file
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileName">File name.</param>
		/// <param name = "settings">Json conversion settings</param>
		public static S FromFile<S> (string fileName, JsonSerializerSettings settings)
			where S : IdentificableManager<T>
		{
			string jsonStr;
			try
			{
				var file = File.OpenText (fileName);
				jsonStr = file.ReadToEnd ();
				file.Close ();
			}
			catch (IOException ex)
			{
				throw new IOException ("Cannot read json file: " + fileName, ex);
			}

			var ret = JsonConvert.DeserializeObject<S> (jsonStr, settings);

			return ret;
		}
	}
}