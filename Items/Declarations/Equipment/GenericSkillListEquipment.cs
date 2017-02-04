using System.Collections.Generic;
using Units.Skills;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Weapon that has a inner skills
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
		/// <param name="slot">Slot</param>
		/// <param name="textureName">Texture</param>
		public GenericSkillListEquipment (string nombre,
		                                  IEnumerable<ISkill> invokedSkill, 
		                                  EquipSlot slot,
		                                  string textureName)
			: base (nombre)
		{
			InvokedSkills = invokedSkill;
			Slot = slot;
			TextureName = textureName;
		}

		public override object Clone ()
		{
			return new GenericSkillListEquipment (NombreBase, InvokedSkills, Slot)
			{
				TextureName = TextureName,
				Texture = Texture,
				Color = Color
			};
		}

		/// <summary>
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name="invokedSkill">Invoked skill list.</param>
		/// <param name="slot">Slot</param>
		public GenericSkillListEquipment (string nombre,
		                                  IEnumerable<ISkill> invokedSkill, 
		                                  EquipSlot slot)
			: this (nombre, invokedSkill, slot, nombre)
		{
		}
	}
}