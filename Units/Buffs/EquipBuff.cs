using System;
using System.Collections.Generic;
using Items;
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

		public BuffManager BuffManager { get; }

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
		public string Nombre
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		/// <summary>
		/// Devuelve la textura a usar
		/// </summary>
		/// <value>The name of the base texture.</value>
		public string BaseTextureName
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public BuffManager Manager
		{
			get
			{
				throw new NotImplementedException ();
			}
			set
			{
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IInternalUpdate implementation

		public void Update (float gameTime)
		{
			throw new NotImplementedException ();
		}

		#endregion

		public EquipBuff ()
		{
		}
	}
}

