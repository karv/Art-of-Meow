using System.Collections.Generic;
using Units.Equipment;
using Units.Recursos;

namespace Units.Buffs
{
	public class EquipBuff : IBuff
	{
		Dictionary<string, float> delta { get; }

		public bool IsVisible { get { return false; } }

		/// <summary>
		/// Devuelve el Equipment de la unidad
		/// </summary>
		public EquipmentManager EquipManager { get; }

		public BuffManager BuffManager { get; set; }

		public ManejadorRecursos RecManager { get; }

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

		public EquipBuff (IUnidad unid)
		{
			EquipManager = unid.Equipment;
			BuffManager = unid.Buffs;
			RecManager = unid.Recursos;
		}
	}
}