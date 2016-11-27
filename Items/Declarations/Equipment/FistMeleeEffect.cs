using Items.Declarations.Equipment;
using Skills;
using Units;
using Units.Recursos;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// The melee effect when the user has not equiped melee effect weapon
	/// </summary>
	public class FistMeleeEffect : IMeleeEffect
	{
		/// <summary>
		/// Causes melee effect on a target
		/// </summary>
		/// <param name="user">The user of the melee move</param>
		/// <param name="target">Target.</param>
		public IEffect GetEffect (IUnidad user, IUnidad target)
		{
			var pct = Helper.HitDamageCalculator.GetPctHit (
				          user,
				          target,
				          ConstantesRecursos.CertezaMelee,
				          ConstantesRecursos.EvasiónMelee);
			var ret = new CollectionEffect (user, pct);
			ret.AddEffect (
				new ChangeRecurso (
					user,
					target,
					ConstantesRecursos.Equilibrio,
					-RecursoEquilibro.ReduceValue,
					1),
				true);

			ret.AddEffect (
				new ChangeRecurso (
					user,
					user,
					ConstantesRecursos.Equilibrio,
					-RecursoEquilibro.ReduceValue,
					1),
				true);
			
			var damage = 0.4f * Helper.HitDamageCalculator.Damage (
				             user, target,
				             ConstantesRecursos.Fuerza,
				             ConstantesRecursos.Destreza);

			ret.AddEffect (
				new ChangeRecurso (user, target, ConstantesRecursos.HP, -damage, 1));

			ret.AddEffect (
				new GenerateCooldownEffect (
					user,
					user,
					calcularTiempoMelee (user)),
				true);

			return ret;
		}

		static float calcularTiempoMelee (IUnidad user)
		{
			var dex = user.Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}
	}
}