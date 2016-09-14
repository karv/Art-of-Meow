using Units.Buffs;
using System.Collections.Generic;
using Items;
using System.Linq;
using System.Diagnostics;

namespace Units.Equipment
{
	public sealed class EquipmentManager
	{
		/// <summary>
		/// El poseedor del equipment
		/// </summary>
		/// <value>The owner.</value>
		public IUnidad Owner { get; }

		/// <summary>
		/// Devuelve el buff que representa el equipment.
		/// </summary>
		public IBuff EquipBuff { get; private set; }

		List<IEquipment> equipment { get; }

		public void EquipItem (IEquipment equip)
		{
			if (CurrentSlotCount (equip.Slot) < SlotSize [equip.Slot])
			{
				equipment.Add (equip);
			}
			else
			{
				Debug.WriteLine (
					"Hay un conflicto de equipment con " + equip.Nombre,
					"Equipment Conflict");
			}
		}

		public EquipmentManager ()
		{
			equipment = new List<IEquipment> ();
		}

		public int CurrentSlotCount (EquipSlot slot)
		{
			return equipment.Count (z => z.Slot == slot);
		}

		public static Dictionary<EquipSlot, int> SlotSize;

		static EquipmentManager ()
		{
			SlotSize = new Dictionary<EquipSlot, int> ();
			SlotSize.Add (EquipSlot.None, 0);
			SlotSize.Add (EquipSlot.Head, 1);
			SlotSize.Add (EquipSlot.Body, 1);
		}
	}
}