using Microsoft.Xna.Framework;
using Moggle.Controles;

namespace Items
{
	/// <summary>
	/// Representa la instancia de un objeto
	/// </summary>
	public interface IItem : IComponent, IDibujable
	{
		/// <summary>
		/// Gets the name for the item
		/// </summary>
		/// <value>The nombre.</value>
		string NombreBase { get; }

		/// <summary>
		/// Gets the default texture name
		/// </summary>
		string DefaultTextureName { get; }

		/// <summary>
		/// Gets the default color
		/// </summary>
		Color DefaultColor { get; }
	}
}