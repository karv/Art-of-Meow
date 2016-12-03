using System.Linq;
using System.Collections.Generic;
using System;

namespace Items.Modifiers
{
	public class ItemModifiersManager
	{
		public IItem Item { get; }

		public string GetName ()
		{
			var ret = Modifiers.Aggregate (
				          "",
				          (acc, iter) => agregarNombreModificado (
					          acc,
					          iter));

			return ret;
		}

		static string agregarNombreModificado (string nombreBase, ItemModifier mod)
		{
			switch (mod.NameUsage)
			{
				case ItemModifierNameUsage.Prefix:
					return string.Format ("{0} {1}", mod.Name + nombreBase);
				case ItemModifierNameUsage.Sufix:
					return string.Format ("{0} {1}", mod.Name + nombreBase);
			}
			throw new Exception (string.Format ("{0} not implemented.", mod.NameUsage));
		}


		public List<ItemModifier> Modifiers { get; }

		public float GetTotalModificationOf (string attr)
		{
			return Modifiers.Sum (im => im.GetModificationValueOf (attr));
		}

		public ICollection<ItemModification> SquashMods ()
		{
			var ret = new Dictionary<string, ItemModification> ();
			foreach (var mod in Modifiers)
				foreach (var item in mod.EnumerateMods())
				{
					ItemModification tempMod;
					if (ret.TryGetValue (item.AttributeChangeName, out tempMod))
						ret [item.AttributeChangeName] = item + tempMod;
					else
						ret.Add (item.AttributeChangeName, item);
				}

			return ret.Values;
		}

		public ItemModifiersManager ()
		{
			Modifiers = new List<ItemModifier> ();
		}
	}
}