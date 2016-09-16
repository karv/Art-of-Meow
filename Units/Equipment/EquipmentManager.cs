using Units.Buffs;
using System.Collections.Generic;
using Items;
using System.Linq;
using System.Diagnostics;
using System;

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

		/// <summary>
		/// Equipa un item
		/// </summary>
		public void EquipItem (IEquipment equip)
		{
			if (equip.Owner != null)
				throw new InvalidOperationException ("equip no debe estar equipado para equiparse.");
			if (CurrentSlotCount (equip.Slot) < SlotSize [equip.Slot])
			{
				equipment.Add (equip);
				equip.Owner = this;
				AgregadoEquipment?.Invoke (this, equip);
			}
			else
			{
				Debug.WriteLine (
					"Hay un conflicto de equipment con " + equip.Nombre,
					"Equipment Conflict");
			}
		}

		/// <summary>
		/// Desequipa un item.
		/// </summary>
		/// <param name="equip">Equip.</param>
		public void UnequipItem (IEquipment equip)
		{
			if (equip.Owner != this)
				throw new InvalidOperationException ("Objeto no está equipado.");

			EliminadoEquipment?.Invoke (this, equip);
			equip.Owner = null;
			equipment.Remove (equip);
		}

		/// <summary>
		/// Devuelve el número de items equipados en un slot dado.
		/// </summary>
		public int CurrentSlotCount (EquipSlot slot)
		{
			return equipment.Count (z => z.Slot == slot);
		}


		#region Events

		/// <summary>
		/// Ocurre al agregar un nuevo equipment.
		/// </summary>
		public event EventHandler<IEquipment> AgregadoEquipment;

		/// <summary>
		/// Ocurre al eliminar un equipment.
		/// </summary>
		public event EventHandler<IEquipment> EliminadoEquipment;

		#endregion

		public EquipmentManager (IUnidad owner)
		{
			Owner = owner;
			equipment = new List<IEquipment> ();
		}

		#region Static

		public static Dictionary<EquipSlot, int> SlotSize;

		static EquipmentManager ()
		{
			SlotSize = new Dictionary<EquipSlot, int> ();
			SlotSize.Add (EquipSlot.None, 0);
			SlotSize.Add (EquipSlot.Head, 1);
			SlotSize.Add (EquipSlot.Body, 1);
		}

		#endregion
	}
}