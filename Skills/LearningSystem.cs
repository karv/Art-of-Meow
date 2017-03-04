using System;
using Units;
using Units.Skills;

namespace Skills
{
	/// <summary>
	/// Represents a skill and its relation with a user
	/// </summary>
	public class SkillAbility
	{
		public readonly ISkill Skill;
		public readonly SkillManager Manager;
		public float Ability;

		public bool IsVisible { get { return Skill.IsVisible (Manager.Unidad); } }

		public SkillAbility (ISkill skill, SkillManager manager)
		{
			Skill = skill;
			Ability = 1;
			Manager = manager;
		}
	}

	public class LearningSystem
	{
		public Unidad Unidad { get; private set; }

		public ISkill CurrentlyLearning { get; private set; }

		float accumulatedKnowledge;

		public float AccumulatedKnowledge
		{
			get
			{
				return accumulatedKnowledge;
			}
			private set
			{
				if (value < 0)
					throw new InvalidOperationException ();
				accumulatedKnowledge = value;
			}
		}

		public float NeededKnowledge { get; private set; }

		public void AddKnowledge (float exp)
		{
			if (exp < 0)
				throw new InvalidOperationException ();
			AccumulatedKnowledge += exp;
		}

		bool shouldLearn { get { return AccumulatedKnowledge <= NeededKnowledge; } }

		public void CheckAndApply ()
		{
			if (shouldLearn)
			{
				Unidad.Skills.AddSkill (CurrentlyLearning);
				CurrentlyLearning = null;
				AccumulatedKnowledge -= NeededKnowledge;
			}
		}

		public LearningSystem (Unidad unidad)
		{
			Unidad = unidad;
		}
	}
}