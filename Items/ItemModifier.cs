using System.Collections.Generic;
using System;

namespace Items
{
	public enum ItemModifierNameUsage
	{
		Prefix,
		Sufix
	}

	public struct ItemModification
	{
		public string AttributeChangeName { get; }

		public float Delta { get; }

		public ItemModification (string attrChangeName, float delta)
		{
			AttributeChangeName = attrChangeName;
			Delta = delta;
		}
	}

	public static class ItemModifierGenerator
	{
		public static ItemModification GenerateItemModification ()
		{
			throw new NotImplementedException ();
		}

		public static ItemModifier GenerateItemModifier ()
		{
			throw new NotImplementedException ();
		}
	}

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