using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Moggle.Controles;
using Skills;

namespace Units
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
		List<SkillAbility> skills { get; }

		/// <summary>
		/// The learning system
		/// </summary>
		public readonly LearningSystem Learning;

		/// <summary>
		/// Enumerates the skill types
		/// </summary>
		public IEnumerable<ISkill> Skills { get { return skills.Select (z => z.Skill); } }

		/// <summary>
		/// Determines whether this instance has the specifed skill
		/// </summary>
		/// <param name="skillName">Skill name.</param>
		public bool HasSkill (string skillName)
		{
			return skills.Exists (z => z.Skill.Name == skillName);
		}

		/// <summary>
		/// Determines whether a skill can be learned
		/// </summary>
		public bool IsOpen (ISkill c)
		{
			// Return if this skill is not learned but can be learned
			return (c.IsLearnable && !HasSkill (c.Name) && c.RequiredSkills.All (HasSkill));
		}

		/// <summary>
		/// Gets the ability of a specified skill
		/// </summary>
		public float AbilityOf (ISkill skill)
		{
			return findSkill (skill).Ability;
		}

		/// <summary>
		/// Gets the <see cref="SkillAbility"/> with the specified <see cref="ISkill"/>. <c>null</c> if it does not exist.
		/// </summary>
		SkillAbility findSkill (ISkill skill)
		{
			return skills.First (z => ReferenceEquals (z.Skill, skill));
		}

		/// <summary>
		/// Revisa y devuelve si existe un skill que puede ser usado en este momento por la unidad
		/// </summary>
		/// <returns><c>true</c>, if usable was anyed, <c>false</c> otherwise.</returns>
		public bool AnyUsable ()
		{
			return (Unidad.EnumerateAllSkills ().Any (z => z.IsVisible (Unidad) && z.IsCastable (Unidad)));
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="SkillManager"/> has any skill.
		/// </summary>
		public bool Any { get { return skills.Any (); } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="SkillManager"/> has any visible skill.
		/// </summary>
		/// <value><c>true</c> if any visible; otherwise, <c>false</c>.</value>
		public bool AnyVisible { get { return skills.Any (z => z.IsVisible); } }

		/// <summary>
		/// Initializes the content of the skills
		/// </summary>
		protected void LoadContent (ContentManager manager)
		{
			foreach (var sk in skills)
				sk.Skill.LoadContent (manager);
		}

		void IComponent.LoadContent (ContentManager manager)
		{
			LoadContent (manager);
		}

		/// <summary>
		/// Initializes every skill in the collection
		/// </summary>
		public void Initialize ()
		{
			foreach (var sk in skills)
				sk.Skill.Initialize ();
		}

		/// <summary>
		/// Adds a skill
		/// </summary>
		/// <param name="skill">Skill.</param>
		public void AddSkill (ISkill skill)
		{
			if (skill == null)
				throw new System.ArgumentNullException ("skill");
			skills.Add (new SkillAbility (skill, this));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SkillManager"/> class.
		/// </summary>
		/// <param name="unid">Unidad</param>
		public SkillManager (IUnidad unid)
		{
			Unidad = unid;
			skills = new List<SkillAbility> ();
			Learning = new LearningSystem (Unidad);
		}

		/// <summary>
		/// The name prefix for skills (for global uniqueness)
		/// </summary>
		public const string namePrefix = "skill.";

		/// <summary>
		/// Gets the global unique name for a skill
		/// </summary>
		/// <returns>The global unique identifier.</returns>
		/// <param name="skl">Skl.</param>
		public static string GetGlobalUniqueId (ISkill skl)
		{// THINK: move this to ISkill ext
			return namePrefix + skl.Name;
		}
	}
}