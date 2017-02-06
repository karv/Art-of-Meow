using AoM;

namespace Items
{
	/// <summary>
	/// Creates an item of a given type
	/// </summary>
	public class SimpleItemRecipe : IItemFactory
	{
		/// <summary>
		/// Type of item
		/// </summary>
		public string ItemName;

		/// <summary>
		/// Creates an item
		/// </summary>
		public IItem Create ()
		{
			return Program.MyGame.Items.CreateItem (ItemName);
		}
	}
}