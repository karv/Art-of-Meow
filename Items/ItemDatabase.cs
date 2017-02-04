using Newtonsoft.Json;

namespace Items
{
	public class ItemDatabase
	{
		public readonly IItem [] Collection;

		[JsonConstructor]
		public ItemDatabase (IItem [] Collection)
		{
			this.Collection = Collection;
		}
	}
}