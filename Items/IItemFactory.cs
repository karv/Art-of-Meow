namespace Items
{
	/// <summary>
	/// Creates items
	/// </summary>
	public interface IItemFactory
	{
		/// <summary>
		/// Creates an item
		/// </summary>
		IItem Create ();
	}
}