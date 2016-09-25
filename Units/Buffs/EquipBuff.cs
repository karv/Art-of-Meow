using System.Collections.Generic;
using System.Linq;
using Units.Recursos;
using Items;

namespace Units.Buffs
{
	public class EquipBuff : IStatsBuff
	{
		ICollection<IEquipment> equipment;

		public bool IsVisible { get { return false; } }

		/// <summary>
		/// Devuelve el Equipment de la unidad
		/// </summary>
		//public EquipmentManager EquipManager { get; }

		public BuffManager BuffManager { get; set; }

		public ManejadorRecursos RecManager { get; }

		public float StatDelta (string resName)
		{
			var ret = 0f;
			foreach (var x in equipment.OfType<IBuffGenerating> ())
			{
				foreach (var buffEntry in x.GetDeltaStat ())
				{
					if (buffEntry.Key == resName)
					{
						ret += buffEntry.Value;
						break;
					}
				}
			}
			return ret;
		}

		#region IBuff implementation

		/// <summary>
		/// Se invoca cuando está por desanclarse
		/// </summary>
		public void Terminating ()
		{
		}

		/// <summary>
		/// De invoca justo después de anclarse.
		/// </summary>
		public void Initialize ()
		{
		}

		/// <summary>
		/// Nombre
		/// </summary>
		/// <value>The nombre.</value>
		public string Nombre { get { return "Equipment"; } }

		/// <summary>
		/// Devuelve la textura a usar
		/// </summary>
		/// <value>The name of the base texture.</value>
		public string BaseTextureName { get { return null; } }

		BuffManager IBuff.Manager
		{
			get { return BuffManager; }
			set { BuffManager = value; }
		}

		#endregion

		#region IInternalUpdate implementation

		public void Update (float gameTime)
		{
		}

		#endregion

		public override string ToString ()
		{
			return string.Format (
				"[EquipBuff: equipment={0}, BuffManager={1}, RecManager={2}, Nombre={3}]",
				equipment,
				BuffManager,
				RecManager,
				Nombre);
		}

		public EquipBuff (ICollection<IEquipment> eqs)
		{
			equipment = eqs;
		}
	}
}