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

		/// <summary>
		/// Revisa y devuelve si existe un skill que puede ser usado en este momento por la unidad
		/// </summary>
		/// <returns><c>true</c>, if usable was anyed, <c>false</c> otherwise.</returns>
		public bool AnyUsable ()
		{
			return (Unidad.EnumerateAllSkills ().Any (z => z.IsVisible (Unidad) && z.IsCastable (Unidad)));
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Units.Skills.SkillManager"/> has any skill.
		/// </summary>
		public bool Any { get { return Skills.Any (); } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Units.Skills.SkillManager"/> has any visible skill.
		/// </summary>
		/// <value><c>true</c> if any visible; otherwise, <c>false</c>.</value>
		public bool AnyVisible { get { return Skills.Any (z => z.IsVisible (Unidad)); } }

		/// <summary>
		/// Loads the content of each <see cref="ISkill"/>
		/// </summary>
		protected void AddContent ()
		{
			foreach (var sk in Skills)
				sk.AddContent ();
		}

		/// <summary>
		/// Initializes the content of the skills
		/// </summary>
		protected void InitializeContent ()
		{
			foreach (var sk in Skills)
				sk.InitializeContent ();
		}

		void IComponent.InitializeContent ()
		{
			InitializeContent ();
		}

		void IComponent.AddContent ()
		{
			AddContent ();
		}

		/// <summary>
		/// Initializes every skill in the collection
		/// </summary>
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