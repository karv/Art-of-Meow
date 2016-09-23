using System.Collections.Generic;
using System.Linq;
using Items;
using Moggle.Controles;

namespace Items
{
	public interface IInventory : IComponent
	{
		ICollection<IItem> Items { get; }

		ILookup<string, IItem> Group ();
	}
}