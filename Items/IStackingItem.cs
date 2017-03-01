using Items;

namespace Items
{
	/// <summary>
	/// Represents an item that is stacked several times while sharing all the properties
	/// </summary>
	public interface IStackingItem : IItem
	{
		/// <summary>
		/// Quantity of items
		/// </summary>
		int Quantity { get; set; }
	}
}