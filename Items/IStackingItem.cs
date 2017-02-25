using Items;

namespace Items
{
	public interface IStackingItem : IItem
	{
		int Quantity { get; set; }
	}
}