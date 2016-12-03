using System.Linq;
using System.Collections.Generic;
using System;

namespace Items.Modifiers
{
	/// <summary>
	/// Controla las modificaciones de un objeto
	/// </summary>
	public class ItemModifiersManager
	{
		/// <summary>
		/// Objeto
		/// </summary>
		/// <value>The item.</value>
		public IItem Item { get; }

		/// <summary>
		/// Devuelve el nombre de objeto, tomando en cuenta sus modificadores
		/// </summary>
		/// <returns>The name.</returns>
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

		/// <summary>
		/// Devuelve los modificadores
		/// </summary>
		public List<ItemModifier> Modifiers { get; }

		/// <summary>
		/// Devuelve la modificación total de un atributo
		/// </summary>
		/// <param name="attr">Nombre del atributo</param>
		public float GetTotalModificationOf (string attr)
		{
			return Modifiers.Sum (im => im.GetModificationValueOf (attr));
		}

		/// <summary>
		/// Devuelve una colección de <see cref="ItemModification"/> en su forma más compacta
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Modifiers.ItemModifiersManager"/> class.
		/// </summary>
		public ItemModifiersManager (IItem item)
		{
			Modifiers = new List<ItemModifier> ();
			Item = item;
		}
	}
}