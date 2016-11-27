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

		/// <summary>
		/// La colección de efectos
		/// </summary>
		public readonly CollectionEffect Effects;

		/// <summary>
		/// Ejecuta
		/// </summary>
		public void Execute ()
		{
			Effects.Execute ();
			//Effects.ExecuteAll ();
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
			Effects = new CollectionEffect (agent);
		}
	}
}