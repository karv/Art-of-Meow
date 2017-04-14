using System;
using Items;
using Skills;
using Units;

namespace Skills
{
	/// <summary>
	/// Representa el efecto de un <see cref="ISkill"/> que consisnte en eliminar un item de un <see cref="IUnidad"/>
	/// </summary>
	public class RemoveItemEffect : Effect, ITargetEffect
	{
		readonly int quantity;

		/// <summary>
		/// Se invoca cuando acierta, elimina el item
		/// </summary>
		protected override void WhenHit ()
		{
			var stackItem = RemovingItem as IStackingItem;
			if (stackItem != null && stackItem.Quantity >= quantity)
				stackItem.Quantity -= quantity;
			else if (!Target.Inventory.Items.Remove (RemovingItem))
				throw new Exception ("Cannot execute effect.\nItem does not exist.\n" + RemovingItem);
		}

		/// <summary>
		/// Se invoca cuando falla
		/// </summary>
		protected override void WhenMiss ()
		{
			// Does nothing
		}

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		public override string DetailedInfo ()
		{
			return string.Format ("{1}: Removes {0}", RemovingItem.NombreBase, Chance);
		}

		/// <summary>
		/// Whose inventory gonna lose the item.
		/// </summary>
		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		/// <summary>
		/// Item to remove
		/// </summary>
		public IItem RemovingItem { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.RemoveItemEffect"/> class.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="target">Target.</param>
		/// <param name="removingItem">Removing item.</param>
		public RemoveItemEffect (IEffectAgent source,
		                         IUnidad target,
		                         IItem removingItem)
			: base (source)
		{
			Target = target;
			RemovingItem = removingItem;
			Chance = 1;
			quantity = 1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.RemoveItemEffect"/> class.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="target">Target.</param>
		/// <param name="removingItem">Removing item.</param>
		/// <param name = "removeQuantity">Quantity to remove</param>
		public RemoveItemEffect (IEffectAgent source,
		                         IUnidad target,
		                         IStackingItem removingItem, 
		                         int removeQuantity)
			: base (source)
		{
			Target = target;
			RemovingItem = removingItem;
			Chance = 1;
			quantity = removeQuantity;
		}
	}
}