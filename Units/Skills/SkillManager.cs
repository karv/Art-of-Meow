using System.Collections.Generic;

namespace Units.Skills
{
	public class SkillManager
	{
		public IUnidad Unidad { get; }

		public List<ISkill> Skills { get; }

		public SkillManager (IUnidad unid)
		{
			Unidad = unid;
			Skills = new List<ISkill> ();
		}
	}
}