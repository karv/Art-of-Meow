using System.Collections.Generic;
using System.Linq;
using Moggle.Controles;

namespace Units.Skills
{
	public class SkillManager : IComponent
	{
		public IUnidad Unidad { get; }

		public List<ISkill> Skills { get; }

		public bool Any { get { return Skills.Any (); } }

		public bool AnyVisible { get { return Skills.Any (z => z.IsVisible (Unidad)); } }

		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			foreach (var sk in Skills)
				sk.LoadContent (manager);
		}

		public void UnloadContent ()
		{
			foreach (var sk in Skills)
				sk.UnloadContent ();
		}

		public void Dispose ()
		{
			UnloadContent ();
		}

		public void Initialize ()
		{
			foreach (var sk in Skills)
				sk.Initialize ();
		}

		public SkillManager (IUnidad unid)
		{
			Unidad = unid;
			Skills = new List<ISkill> ();
		}
	}
}