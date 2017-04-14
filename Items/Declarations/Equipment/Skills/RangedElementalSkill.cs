using System;
using Helper;
using Newtonsoft.Json;
using Skills;
using Units;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	/// <summary>
	/// Ranged skill causing elemental damage
	/// </summary>
	public class RangedElementalSkill : RangedSkill
	{
		/// <summary>
		/// The base hit.
		/// </summary>
		public float BaseHit = 1;
		/// <summary>
		/// Attribute name
		/// </summary>
		public string Attribute;
		/// <summary>
		/// Final damage multiplier
		/// </summary>
		public float DamageMultiplier = 1;
		/// <summary>
		/// Base cooldown.
		/// </summary>
		public float BaseCooldown;

		/// <summary>
		/// Name of the consuming resource
		/// </summary>
		public string RecursoUsageName;
		/// <summary>
		/// Quantity of resource consumed (cast)
		/// </summary>
		public float RecursoUsageQuantity;

		/// <summary>
		/// Is castable when can pay Recursousage
		/// </summary>
		public override bool IsCastable (IUnidad user)
		{
			if (RecursoUsageName == null)
				return true;
			
			return user.Recursos.ValorRecurso (RecursoUsageName) >= RecursoUsageQuantity;
		}

		/// <summary>
		/// Determines whether this skill can be learned
		/// </summary>
		protected override bool IsLearnable { get { return true; } }

		/// <summary>
		/// Builds the instance
		/// </summary>
		/// <returns>The skill instance.</returns>
		/// <param name="user">User.</param>
		/// <param name="target">Target.</param>
		public override SkillInstance BuildSkillInstance (IUnidad user, IUnidad target)
		{
			if (target == null)
				throw new ArgumentNullException ("target");
			if (user == null)
				throw new ArgumentNullException ("user");

			var chance = HitDamageCalculator.GetPctHit (
				             user,
				             target,
				             ConstantesRecursos.CertezaRango,
				             ConstantesRecursos.EvasiónRango,
				             BaseHit);
			var dmg = HitDamageCalculator.Damage (
				          user,
				          target,
				          ConstantesRecursos.Fuerza,
				          ConstantesRecursos.Fuerza, Attribute);
			dmg *= DamageMultiplier;

			var ef = new ChangeRecurso (
				         user,
				         target,
				         ConstantesRecursos.HP,
				         -dmg, 
				         chance);

			var ret = new SkillInstance (this, user);
			ret.Effects.Chance = chance;
			ret.Effects.AddEffect (ef);
			ret.Effects.AddEffect (new GenerateCooldownEffect (user, user, BaseCooldown), true);
			return ret;
		}

		/// <summary>
		/// Invoked when this skill hit
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="target">Target.</param>
		protected override void OnHit (IUnidad user, IUnidad target)
		{
			var rec = user.Recursos.GetRecurso (RecursoUsageName);
			rec.Valor -= RecursoUsageQuantity;
			user.Exp.AddAssignation (SkillManager.GetGlobalUniqueId (this), 1);
		}

		/// <summary>
		/// Invoked when this skill miss
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="target">Target.</param>
		protected override void OnMiss (IUnidad user, IUnidad target)
		{
			var rec = user.Recursos.GetRecurso (RecursoUsageName);
			rec.Valor -= RecursoUsageQuantity;
			user.Exp.AddAssignation (SkillManager.GetGlobalUniqueId (this), 1);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.Skills.RangedElementalSkill"/> class.
		/// </summary>
		/// <param name="Name">Name.</param>
		/// <param name="TextureName">Texture name.</param>
		/// <param name="Icon">Icon.</param>
		[JsonConstructor]
		public RangedElementalSkill (string Name,
		                             string TextureName,
		                             string Icon)
			: base (Name,
			        TextureName,
			        Icon)
		{
		}
	}
}