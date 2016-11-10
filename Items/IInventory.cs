using System.Collections.Generic;
using System.Linq;
using Items;
using Moggle.Controles;

namespace Items
{
	/// <summary>
	/// Manages the inventory of a <see cref="Units.IUnidad"/>
	/// </summary>
	public interface IInventory : IComponent
	{
		/// <summary>
		/// Gets the items
		/// </summary>
		/// <value>The items.</value>
		ICollection<IItem> Items { get; }

		/// <summary>
		/// Groups the items by type
		/// </summary>
		ILookup<string, IItem> Group ();
	}
}