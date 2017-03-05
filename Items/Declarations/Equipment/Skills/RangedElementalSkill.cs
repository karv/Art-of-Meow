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

		protected override void OnHit (EffectResultEnum result, IUnidad user, IUnidad target)
		{
			throw new NotImplementedException ();
		}

		protected override void OnMiss (EffectResultEnum result, IUnidad user, IUnidad target)
		{
			throw new NotImplementedException ();
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