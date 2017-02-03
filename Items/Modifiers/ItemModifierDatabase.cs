using System;
using Newtonsoft.Json;

namespace Items.Modifiers
{
	/// <summary>
	/// Manages the entire collection of <see cref="ItemModifier"/>
	/// </summary>
	public class ItemModifierDatabase
	{
		//THINK: Should be loaded from file every time or stored in memory?
		/// <summary>
		/// The array with all the modifications
		/// </summary>
		[JsonProperty ("Mods")]
		public readonly ItemModifier [] Mods;

		[JsonConstructor]
		ItemModifierDatabase (ItemModifier [] Mods)
		{
			this.Mods = Mods;
		}
	}
}