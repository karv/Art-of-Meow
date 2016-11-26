using System.Collections.Generic;
using Skills;
using Units.Skills;

namespace Skills
{
	/// <summary>
	/// Una instancia de skill.
	/// Se usa como feedback de datos y resultados del skill
	/// </summary>
	public class SkillInstance
	{
		/// <summary>
		/// Tipo de skill
		/// </summary>
		public ISkill Skill { get; }

		/// <summary>
		/// El agente o usuario del skill
		/// </summary>
		public IEffectAgent Agent { get; }

		readonly List<IEffect> _effects;

		/// <summary>
		/// Añadir un efecto
		/// </summary>
		public void AddEffect (IEffect effect)
		{
			_effects.Add (effect);
		}

		/// <summary>
		/// Ejecuta
		/// </summary>
		public void Execute ()
		{
			for (int i = 0; i < _effects.Count; i++)
				_effects [i].Execute (true);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.SkillInstance"/> class.
		/// </summary>
		/// <param name="skill">Skill.</param>
		/// <param name="agent">Agent.</param>
		public SkillInstance (ISkill skill, IEffectAgent agent)
		{
			Skill = skill;
			Agent = agent;
			_effects = new List<IEffect> ();
		}
	}
}