using System.Collections.Generic;
using System;

namespace Items.Modifiers
{
	public sealed class ItemModifier
	{
		public ItemModifierNameUsage NameUsage { get; }

		Dictionary<string, ItemModification> Modifications { get; }

		public IEnumerable<ItemModification> EnumerateMods ()
		{
			return Modifications.Values;
		}

		public float GetModificationValueOf (string attr)
		{
			var mod = GetModificationOf (attr);
			return mod.HasValue ? mod.Value.Delta : 0f;
		}

		public ItemModification? GetModificationOf (string attr)
		{
			ItemModification mod;
			return Modifications.TryGetValue (attr, out mod) ? mod : new ItemModification? ();
		}


		public string Name { get; }

		public ItemModifier ()
		{
			Modifications = new Dictionary<string, ItemModification> ();
		}
	}
}