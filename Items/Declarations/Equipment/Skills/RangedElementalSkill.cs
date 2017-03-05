using System;
using Helper;
using Skills;
using Units;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	public class RangedElementalSkill : RangedSkill
	{
		public float BaseHit = 1;
		public string Attribute;
		public float DamageMultiplier = 1;
		public float BaseCooldown;

		public string RecursoUsageName;
		public float RecursoUsageQuantity;

		public override bool IsCastable (IUnidad user)
		{
			if (RecursoUsageName == null)
				return true;
			
			return user.Recursos.ValorRecurso (RecursoUsageName) >= RecursoUsageQuantity;
		}

		protected override bool IsLearnable { get { return true; } }

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

		protected override void OnHit (IUnidad user, IUnidad target)
		{
			var rec = user.Recursos.GetRecurso (RecursoUsageName);
			rec.Valor -= RecursoUsageQuantity;
			user.Exp.AddAssignation (SkillManager.GetGlobalUniqueId (this), 1);
		}

		protected override void OnMiss (IUnidad user, IUnidad target)
		{
			var rec = user.Recursos.GetRecurso (RecursoUsageName);
			rec.Valor -= RecursoUsageQuantity;
			user.Exp.AddAssignation (SkillManager.GetGlobalUniqueId (this), 1);
		}

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