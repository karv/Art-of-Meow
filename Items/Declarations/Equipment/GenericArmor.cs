using System.Collections.Generic;
using Units.Buffs;

namespace Items.Declarations.Equipment
{
	public class GenericArmor : Equipment, IBuffGenerating
	{
		public Dictionary<string, float> DeltaDict { get; }

		public string TextureNameGeneric { get { return TextureName; } set { TextureName = value; } }

		public override EquipSlot Slot { get; }

		public IEnumerable<KeyValuePair<string, float>> GetDeltaStat ()
		{
			return DeltaDict;
		}

		public GenericArmor (string nombre, EquipSlot slot)
			: base (nombre)
		{
			DeltaDict = new Dictionary<string, float> ();
			Slot = slot;
		}
	}
}
