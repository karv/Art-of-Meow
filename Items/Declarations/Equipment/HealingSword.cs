using System;
using Units.Skills;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Obsolete
	/// </summary>
	[ObsoleteAttribute]
	public class HealingSword : Sword, ISkillEquipment
	{
		readonly SelfHealSkill healSkill;

		/// <summary>
		/// Loads the texture
		/// </summary>
		protected override void AddContent ()
		{
			base.AddContent ();
			healSkill.AddContent ();
		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		protected override void InitializeContent ()
		{
			base.InitializeContent ();
			healSkill.InitializeContent ();
		}

		System.Collections.Generic.IEnumerable<ISkill> ISkillEquipment.GetEquipmentSkills ()
		{
			return new ISkill[] { healSkill };
		}

		/// <summary>
		/// </summary>
		public HealingSword ()
		{
			healSkill = new SelfHealSkill ();
		}
	}
}