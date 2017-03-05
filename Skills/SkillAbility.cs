using System;
using Units;
using Units.Skills;

namespace Skills
{
	/// <summary>
	/// Represents a skill and its relation with a user
	/// </summary>
	public class SkillAbility : IExpable
	{
		public readonly ISkill Skill;
		public readonly SkillManager Manager;
		public float Ability;

		public bool IsVisible { get { return Skill.IsVisible (Manager.Unidad); } }

		#region IExpable implementation

		void IExpable.ReceiveExperience (float exp)
		{
			Ability += exp;
		}

		string IExpable.NombreÚnico
		{
			get { return SkillManager.GetGlobalUniqueId (Skill); }
		}

		#endregion

		public SkillAbility (ISkill skill, SkillManager manager)
		{
			if (manager == null)
				throw new ArgumentNullException ("manager");
			if (skill == null)
				throw new ArgumentNullException ("skill");
			
			Skill = skill;
			Ability = 1;
			Manager = manager;
		}
	}
}