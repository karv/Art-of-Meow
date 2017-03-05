using System;
using System.Linq;
using Helper;
using Newtonsoft.Json;
using Skills;
using Units;
using Units.Recursos;

namespace Items.Declarations.Equipment.Skills
{
	public class RangedAmmoSkill : RangedSkill
	{
		/// <summary>
		/// Builds the instance
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="target">Target.</param>
		public override SkillInstance BuildSkillInstance (IUnidad user, IUnidad target)
		{
			var quiver = user.Equipment.EquipmentInSlot (EquipSlot.Quiver).OfType<Arrow> ();
			if (!quiver.Any ())
				throw new Exception ("Cannot invoke ranged skill without ammo.");
			var arrow = quiver.First ();

			if (target == null)
				throw new ArgumentNullException ("target");
			if (user == null)
				throw new ArgumentNullException ("user");

			var baseHit = arrow.BaseHit;
			var chance = HitDamageCalculator.GetPctHit (
				             user,
				             target,
				             ConstantesRecursos.CertezaRango,
				             ConstantesRecursos.EvasiónRango,
				             baseHit);
			var dmg = HitDamageCalculator.Damage (
				          user,
				          target,
				          ConstantesRecursos.Fuerza,
				          ConstantesRecursos.Fuerza, arrow.Attribute);
			dmg *= arrow.DamageMultiplier;

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
			ret.Effects.AddEffect (new RemoveItemEffect (user, user, arrow, 1));

			return ret;
		}

		public float BaseCooldown;

		protected override void OnHit (IUnidad user, IUnidad target)
		{
			user.Exp.AddAssignation (ConstantesRecursos.CertezaRango, "base", 0.4f);
			target.Exp.AddAssignation (ConstantesRecursos.EvasiónRango, "base", 0.2f);
			target.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor -= 0.1f;
		}

		protected override void OnMiss (IUnidad user, IUnidad target)
		{
			user.Exp.AddAssignation (ConstantesRecursos.CertezaRango, "base", 0.2f);
			target.Exp.AddAssignation (ConstantesRecursos.EvasiónRango, "base", 0.4f);
			target.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor -= 0.2f;
		}

		[JsonConstructor]
		protected RangedAmmoSkill (string Name, string TextureName, string Icon, float BaseCooldown, string Attribute)
			: base (Name,
			        TextureName,
			        Icon)
		{
		}
	}
	
}