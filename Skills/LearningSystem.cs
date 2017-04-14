using System;
using Skills;
using Units;

namespace Skills
{
	/// <summary>
	/// System used for each human payer to learn new skills
	/// </summary>
	public class LearningSystem
	{
		/// <summary>
		/// Unidad
		/// </summary>
		public IUnidad Unidad { get; private set; }

		ISkill currentlyLearning;

		/// <summary>
		/// Gets or sets the currently learning skill
		/// </summary>
		public ISkill CurrentlyLearning
		{
			get
			{
				return currentlyLearning;
			}
			set
			{
				if (currentlyLearning != null && value != null)
					throw new Exception ();
				currentlyLearning = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is learning something
		/// </summary>
		public bool IsLearning { get { return CurrentlyLearning != null; } }

		float accumulatedKnowledge;

		/// <summary>
		/// Gets the accumulated knowledge.
		/// </summary>
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

		/// <summary>
		/// Gets the needed knowledge for this skill
		/// </summary>
		public float NeededKnowledge { get; private set; }

		/// <summary>
		/// Adds knowledge
		/// </summary>
		public void AddKnowledge (float exp)
		{
			if (exp < 0)
				throw new InvalidOperationException ();
			AccumulatedKnowledge += exp;
		}

		bool shouldLearn { get { return AccumulatedKnowledge <= NeededKnowledge && CurrentlyLearning != null; } }

		/// <summary>
		/// Checks whether learns the current skill, if positive, does so
		/// </summary>
		public void CheckAndApply ()
		{
			if (shouldLearn)
			{
				Unidad.Skills.AddSkill (CurrentlyLearning);
				CurrentlyLearning = null;
				AccumulatedKnowledge -= NeededKnowledge;
			}
		}

		/// <summary>
		/// </summary>
		public LearningSystem (IUnidad unidad)
		{
			Unidad = unidad;
		}
	}
}