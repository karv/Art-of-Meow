using Newtonsoft.Json;

namespace AoM
{
	/// <summary>
	/// Represents an attribute (elemental, etc) as damage type
	/// </summary>
	public class DamageAttribute : IIdentificable
	{
		public string Name { get; }

		[JsonConstructor]
		DamageAttribute (string Name)
		{
			this.Name = Name;
		}
	}
}