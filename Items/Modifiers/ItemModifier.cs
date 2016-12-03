using System.Collections.Generic;
using System;

namespace Items.Modifiers
{
	/// <summary>
	/// Representa un modificador de items, que puede tener varias modificaciones a distintos atributos
	/// </summary>
	public sealed class ItemModifier
	{
		/// <summary>
		/// Devuelve un <see cref="ItemModifierNameUsage"/> que determina cómo concatenar el nombre del objeto
		/// con el nombre de la modificación
		/// </summary>
		public ItemModifierNameUsage NameUsage { get; }

		Dictionary<string, ItemModification> Modifications { get; }

		/// <summary>
		/// Enumera los modificadores
		/// </summary>
		public IEnumerable<ItemModification> EnumerateMods ()
		{
			return Modifications.Values;
		}

		/// <summary>
		/// Devuelve el valor de modificación de un atributo
		/// </summary>
		/// <returns>The modification value of.</returns>
		/// <param name="attr">Nombre del atributo</param>
		public float GetModificationValueOf (string attr)
		{
			var mod = GetModificationOf (attr);
			return mod.HasValue ? mod.Value.Delta : 0f;
		}

		/// <summary>
		/// Devuelve el modificador que cambia un atributo dado
		/// </summary>
		/// <returns>El modificador de un atributo; o <c>null</c> si éste no existe</returns>
		/// <param name="attr">Attr.</param>
		public ItemModification? GetModificationOf (string attr)
		{
			ItemModification mod;
			return Modifications.TryGetValue (attr, out mod) ? mod : new ItemModification? ();
		}

		/// <summary>
		/// Devuelve el nombre del modificador
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Modifiers.ItemModifier"/> class.
		/// </summary>
		public ItemModifier (string name,
		                     ItemModifierNameUsage GramUse,
		                     IEnumerable<ItemModification> mods)
		{
			Modifications = new Dictionary<string, ItemModification> ();
			Name = name;
			NameUsage = GramUse;
			foreach (var mod in mods)
				Modifications.Add (mod.AttributeChangeName, mod);
		}
	}
}