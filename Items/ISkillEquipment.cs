using System.Collections.Generic;
using Skills;

namespace Items
{
	/// <summary>
	/// Un equipamento que ofrece Skills al usuario
	/// </summary>
	public interface ISkillEquipment : IEquipment
	{
		/// <summary>
		/// Devuelve la lista de skills que ofrece al usuario
		/// </summary>
		IEnumerable<ISkill> GetEquipmentSkills ();
	}
}