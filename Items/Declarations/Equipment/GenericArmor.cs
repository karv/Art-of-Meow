using System.Collections.Generic;
using Units.Buffs;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Armadura equipable genérica.
	/// Puede cambiar los stats asignando un diccionario
	/// </summary>
	public class GenericArmor : Equipment, IBuffGenerating
	{
		/// <summary>
		/// Diccionario que contiene los cambios en los stats (por nombre único) en quien lo lleva puesto
		/// </summary>
		/// <value>The delta dict.</value>
		protected Dictionary<string, float> DeltaDict { get; }

		/// <summary>
		/// Devuelve o establece el nombre de la textura a usar
		/// </summary>
		public string TextureNameGeneric { get { return TextureName; } set { TextureName = value; } }

		/// <summary>
		/// Slot que ocupa esta armadura
		/// </summary>
		public override EquipSlot Slot { get; }

		IEnumerable<KeyValuePair<string, float>> IBuffGenerating.GetDeltaStat ()
		{
			return DeltaDict;
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override object Clone ()
		{
			return new GenericArmor (NombreBase, Slot)
			{
				TextureName = TextureName,
				Texture = Texture,
				Color = Color,
				AllowedModNames = AllowedModNames
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.GenericArmor"/> class.
		/// </summary>
		/// <param name="nombre">Nombre</param>
		/// <param name="slot">Slot</param>
		public GenericArmor (string nombre, EquipSlot slot)
			: base (nombre)
		{
			DeltaDict = new Dictionary<string, float> ();
			Slot = slot;
		}
	}
}