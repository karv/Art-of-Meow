using System;
using Units;
using Skills;
using Units.Recursos;
using System.Collections.Generic;
using AoM;

namespace Skills
{
	public class LearningSystem
	{
		public IUnidad Unidad { get; private set; }

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

		bool shouldLearn { get { return AccumulatedKnowledge <= NeededKnowledge && CurrentlyLearning != null; } }

		public void CheckAndApply ()
		{
			if (shouldLearn)
			{
				Unidad.Skills.AddSkill (CurrentlyLearning);
				CurrentlyLearning = null;
				AccumulatedKnowledge -= NeededKnowledge;
			}
		}

		public LearningSystem (IUnidad unidad)
		{
			Unidad = unidad;
		}
	}
}