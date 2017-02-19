using System;
using AoM;
using Items.Modifiers;
using Microsoft.Xna.Framework;
using Moggle.Controles;

namespace Items
{
	/// <summary>
	/// Representa la instancia de un objeto
	/// </summary>
	public interface IItem : IComponent, IDibujable, ICloneable, IIdentificable
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

		/// <summary>
		/// Gets the allowed modifications for this item
		/// </summary>
		ItemModifier[] AllowedMods { get; }

		/// <summary>
		/// Gets the modifier manager
		/// </summary>
		ItemModifiersManager Modifiers { get; }
	}
}