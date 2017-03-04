﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Moggle.Controles;
using Skills;

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
		List<SkillAbility> skills { get; }

		public IEnumerable<ISkill> Skills { get { return skills.Select (z => z.Skill); } }

		public float AbilityOf (ISkill skill)
		{
			return findSkill (skill).Ability;
		}

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
		/// Gets a value indicating whether this <see cref="Units.Skills.SkillManager"/> has any skill.
		/// </summary>
		public bool Any { get { return skills.Any (); } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="Units.Skills.SkillManager"/> has any visible skill.
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

		public void AddSkill (ISkill skill)
		{
			skills.Add (new SkillAbility (skill, this));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Skills.SkillManager"/> class.
		/// </summary>
		/// <param name="unid">Unidad</param>
		public SkillManager (IUnidad unid)
		{
			Unidad = unid;
			skills = new List<SkillAbility> ();
		}
	}
}