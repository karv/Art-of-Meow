using System;

namespace Items.Modifiers
{
	/// <summary>
	/// Representa una modificación de un sólo atributo como parte de un <see cref="ItemModifier"/>
	/// </summary>
	public struct ItemModification
	{
		/// <summary>
		/// Devuelve el nombre del atributo que es afectado
		/// </summary>
		public string AttributeChangeName { get; }

		/// <summary>
		/// Devuelve el valor absoluto de la modificación
		/// </summary>
		public float Delta { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Modifiers.ItemModification"/> struct.
		/// </summary>
		public ItemModification (string attrChangeName, float delta)
		{
			AttributeChangeName = attrChangeName;
			Delta = delta;
		}

		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static ItemModification operator + (ItemModification left,
		                                           ItemModification right)
		{
			if (left.AttributeChangeName != right.AttributeChangeName)
				throw new InvalidOperationException ();
			return new ItemModification (
				left.AttributeChangeName,
				left.Delta + right.Delta);
		}
	}
}