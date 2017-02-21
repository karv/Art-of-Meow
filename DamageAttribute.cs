using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace AoM
{
	/// <summary>
	/// Represents an attribute (elemental, etc) as damage type
	/// </summary>
	public class DamageAttribute : IIdentificable
	{
		/// <summary>
		/// Gets the unique name
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets the prefered color for display
		/// </summary>

		public Color DisplayColor { get; }

		[JsonConstructor]
		DamageAttribute (string Name, Color DisplayColor)
		{
			this.Name = Name;
			this.DisplayColor = DisplayColor;
		}
	}
}