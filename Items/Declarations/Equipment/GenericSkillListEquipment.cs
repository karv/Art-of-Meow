using System.Collections.Generic;
using Units.Skills;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Arma genérica de rango
	/// </summary>
	public class GenericSkillListEquipment : Equipment, ISkillEquipment
	{
		/// <summary>
		/// Gets the slot where it can be equiped.
		/// </summary>
		public override EquipSlot Slot { get; }

		/// <summary>
		/// Devuelve el skill que se invoca al usar este item
		/// </summary>
		public IEnumerable <ISkill> InvokedSkills { get; }

		IEnumerable<ISkill> ISkillEquipment.GetEquipmentSkills ()
		{
			return InvokedSkills;
		}

		/// <summary>
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name="invokedSkill">Invoked skill list.</param>
		public GenericSkillListEquipment (string nombre,
		                                  IEnumerable<ISkill> invokedSkill, 
		                                  EquipSlot slot)
			: base (nombre)
		{
			InvokedSkills = invokedSkill;
			Slot = slot;
		}
	}
}