using Newtonsoft.Json;
using Skills;
using System;

namespace Items.Declarations
{
	/// <summary>
	/// An arrow
	/// </summary>
	public class Arrow : StackingItem, IEquipment, IEffectAgent
	{
		/// <summary>
		/// Gets the slot where it can be equiped
		/// </summary>
		/// <value>The slot.</value>
		public EquipSlot Slot
		{
			get { return EquipSlot.Quiver; }
		}

		/// <summary>
		/// Gets the tooltip info (shown in equipment screen)
		/// </summary>
		public override string GetTooltipInfo ()
		{
			return string.Format ("Hit: {0}\tDamage: {1}", BaseHit, DamageMultiplier);
		}

		/// <summary>
		/// The damage mutiplier
		/// </summary>
		public float DamageMultiplier = 1;
		/// <summary>
		/// The base hit chance
		/// </summary>
		public float BaseHit = 0.7f;

		/// <summary>
		/// The name of the damage attribute
		/// </summary>
		public string Attribute;

		Units.Equipment.EquipmentManager IEquipment.Owner { get; set; }

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override object Clone ()
		{
			return new Arrow (NombreBase, AllowedModNames)
			{
				Quantity = Quantity, 
				Attribute = Attribute,
				DamageMultiplier = DamageMultiplier, 
				BaseHit = BaseHit,
				TextureName = TextureName
			};
		}

		[JsonConstructor]
		Arrow (string NombreBase, string [] AllowedModNames)
			: base (NombreBase, AllowedModNames)
		{
		}
	}
}