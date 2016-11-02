using System.Collections.Generic;
using System.Linq;
using Moggle.Controles;
using Moggle;

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
		/// <param name="manager">Manager.</param>
		protected void AddContent (BibliotecaContenido manager)
		{
			foreach (var sk in Skills)
				sk.AddContent (manager);
		}

		protected void InitializeContent (BibliotecaContenido manager)
		{
			foreach (var sk in Skills)
				sk.InitializeContent (manager);
		}

		void IComponent.InitializeContent (BibliotecaContenido manager)
		{
			InitializeContent (manager);
		}

		void IComponent.AddContent (BibliotecaContenido manager)
		{
			AddContent (manager);
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Units.Skills.SkillManager"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Units.Skills.SkillManager"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Units.Skills.SkillManager"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="Units.Skills.SkillManager"/> so
		/// the garbage collector can reclaim the memory that the <see cref="Units.Skills.SkillManager"/> was occupying.</remarks>
		public void Dispose ()
		{
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