using Units.Buffs;
using Units.Recursos;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Una espada sin propiedades especiales
	/// </summary>
	public class Sword : Equipment, IBuffGenerating
	{
		public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, float>> GetDeltaStat ()
		{
			yield return new System.Collections.Generic.KeyValuePair<string, float> (
				ConstantesRecursos.DañoMelee,
				3);
		}

		#region IEquipment implementation

		public override EquipSlot Slot { get { return EquipSlot.MainHand; } }

		#endregion

		protected Sword (string nombre, string icon)
			: base (nombre)
		{
			TextureName = icon;
		}

		public Sword ()
			: this ("Sword", @"Items/katana")
		{
		}
	}
}