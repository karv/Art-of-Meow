using System.Collections.Generic;

namespace Units.Skills
{
	/// <summary>
	/// Skill manager.
	/// </summary>
	public class SkillManager
	{
		/// <summary>
		/// Unidad that has these skills
		/// </summary>
		public IUnidad Unidad { get; }

		/// <summary>
		/// Gets the collection of skills
		/// </summary>
		/// <value>The skills.</value>
		public List<ISkill> Skills { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Skills.SkillManager"/> class.
		/// </summary>
		/// <param name="unid">Unidad</param>
		public SkillManager (IUnidad unid)
		{
			Unidad = unid;
			Skills = new List<ISkill> ();
		}
	}
}