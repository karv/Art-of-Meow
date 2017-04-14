using System;
using Units;

namespace Skills
{
	/// <summary>
	/// Represents a skill and its relation with a user
	/// </summary>
	public class SkillAbility : IExpable
	{
		/// <summary>
		/// The skill
		/// </summary>
		public readonly ISkill Skill;
		/// <summary>
		/// The manager.
		/// </summary>
		public readonly SkillManager Manager;
		/// <summary>
		/// The ability for <see cref="Skill"/>
		/// </summary>
		public float Ability;

		/// <summary>
		/// Gets a value indicating whether the skill is visible.
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.SkillAbility"/> class.
		/// <see cref="Ability"/> is set to 1
		/// </summary>
		/// <param name="skill">Skill.</param>
		/// <param name="manager">Manager.</param>
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