using System.Collections.Generic;
using System.Linq;
using Moggle.Controles;

namespace Units.Skills
{
	/// <summary>
	/// Skill manager.
	/// </summary>
	public class SkillManager : IComponent
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