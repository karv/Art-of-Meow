using System.Collections.Generic;

namespace Items.Modifiers
{
	public sealed class ItemModifier
	{
		public ItemModifierNameUsage NameUsage { get; }

		List<ItemModification> Modifications { get; }

		public string Name { get; }

		public ItemModifier ()
		{
			Modifications = new List<ItemModification> ();
		}
	}
}