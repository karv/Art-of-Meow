using System.Collections.Generic;
using Items;
using Moggle.Controles;

namespace Items
{
	public interface IInventory : IComponent
	{
		ICollection<IItem> Items { get; }
	}
	
}