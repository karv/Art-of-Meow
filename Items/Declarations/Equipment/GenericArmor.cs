using Units.Buffs;
using System.Collections.Generic;

namespace Items.Declarations.Equipment
{
	public abstract class GenericArmor : Equipment, IBuffGenerating
	{
		public Dictionary<string, float> DeltaDict { get; }

		public IEnumerable<KeyValuePair<string, float>> GetDeltaStat ()
		{
			return DeltaDict;
		}

		protected GenericArmor (string nombre)
			: base (nombre)
		{
			DeltaDict = new Dictionary<string, float> ();
		}
		
	}
}
