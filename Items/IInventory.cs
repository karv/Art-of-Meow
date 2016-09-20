using System.Collections.Generic;
using Items;
using Moggle.Controles;
using System.Linq;

namespace Items
{
	public interface IInventory : IComponent
	{
		ICollection<IItem> Items { get; }

		ILookup<string, IItem> Group ();
	}
}