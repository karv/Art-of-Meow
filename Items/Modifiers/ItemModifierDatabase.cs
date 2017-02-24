using AoM;
using Newtonsoft.Json;

namespace Items.Modifiers
{
	/// <summary>
	/// Manages the entire collection of <see cref="ItemModifier"/>
	/// </summary>
	public class ItemModifierDatabase : IdentificableManager<ItemModifier>
	{
		/// <summary>
		/// Gets the modifier with a given name.
		/// Case insensitive.
		/// </summary>
		/// <param name="name">Name.</param>
		public ItemModifier this [string name]
		{
			get
			{
				return Get (name);
			}
		}

		[JsonConstructor]
		ItemModifierDatabase (ItemModifier [] Mods)
			: base (Mods)
		{
		}
	}
}