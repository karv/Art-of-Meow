using System;
using Newtonsoft.Json;

namespace Items.Modifiers
{
	/// <summary>
	/// Manages the entire collection of <see cref="ItemModifier"/>
	/// </summary>
	public class ItemModifierDatabase
	{
		[JsonProperty ("Mods")]
		public readonly ItemModifier [] Mods;

		[JsonConstructor]
		public ItemModifierDatabase (ItemModifier [] Mods)
		{
			this.Mods = Mods;
		}
	}
}