using System;
using System.Diagnostics.Contracts;

namespace Items
{
	public enum ItemModifierNameUsage
	{
		Prefix,
		Sufix
	}

	public sealed class ItemModifier
	{
		public ItemModifierNameUsage NameUsage { get; }

		public string Name { get; }

		public string ApplyToName (string name)
		{
			switch (NameUsage)
			{
				case ItemModifierNameUsage.Prefix:
					return Name + " " + name;
				case ItemModifierNameUsage.Sufix:
					return name + " " + Name;
			}
			throw new Exception ("Unknown usage type");
		}

		public void ApplyToItem (IItem item)
		{
		}
	}
}