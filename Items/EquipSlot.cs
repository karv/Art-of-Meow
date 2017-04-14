
namespace Items
{
	/// <summary>
	/// Represents a slot where a <see cref="IEquipment"/> may be equiped
	/// </summary>
	public enum EquipSlot
	{
		/// <summary>
		/// Cannot be equiped
		/// </summary>
		None = 0,
		/// <summary>
		/// The head.
		/// </summary>
		Head = 1,
		/// <summary>
		/// Body
		/// </summary>
		Body = 2,
		/// <summary>
		/// The dominant hand
		/// </summary>
		MainHand = 3,
		/// <summary>
		/// Arrow, etc
		/// </summary>
		Quiver = 4,
	}
}