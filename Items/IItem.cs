using System;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Items.Modifiers;

namespace Items
{
	/// <summary>
	/// Representa la instancia de un objeto
	/// </summary>
	public interface IItem : IComponent, IDibujable, ICloneable
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

		/// <summary>
		/// Gets the value or worth of the item
		/// </summary>
		float Value { get; }

		ItemModifier[] AllowedMods { get; }

		ItemModifiersManager Modifiers { get; }
	}
}