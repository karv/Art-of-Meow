using System.Linq;
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

		/// <summary>
		/// Gets the modifier with a given name.
		/// Case insensitive.
		/// </summary>
		/// <param name="name">Name.</param>
		public ItemModifier this [string name]
		{
			get
			{
				return Mods.First (z => string.Equals (z.Name, name, System.StringComparison.InvariantCultureIgnoreCase));
			}
		}

		[JsonConstructor]
		ItemModifierDatabase (ItemModifier [] Mods)
		{
			this.Mods = Mods;
		}
	}
}