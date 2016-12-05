using Skills;
using Units;
using Units.Recursos;

namespace Helper
{
	/// <summary>
	/// Provee métodos para construir effectos de daño melee
	/// </summary>
	public static class MeleeEffectHelper
	{
		/// <summary>
		/// Devuelve el efecto default de daño melee
		/// </summary>
		/// <returns>Un <see cref="CollectionEffect"/></returns>
		/// <param name="user">Unidad que causa el efecto</param>
		/// <param name="target">Unidad que recibe el efecto</param>
		/// <param name="baseDamage">Daño base</param>
		/// <param name="baseHit">Probabilidad base de acierto</param>
		/// <param name = "addDefaultCooldownTime">Determina si debe agregar cooldown como efecto</param>
		public static CollectionEffect BuildDefaultMeleeEffect (IUnidad user,
		                                                        IUnidad target,
		                                                        float baseDamage,
		                                                        float baseHit, 
		                                                        bool addDefaultCooldownTime = true)
		{
			var pct = HitDamageCalculator.GetPctHit (
				          user,
				          target,
				          ConstantesRecursos.CertezaMelee,
				          ConstantesRecursos.EvasiónMelee, 
				          baseHit);
			var ret = new CollectionEffect (user, pct);
			ret.AddEffect (
				new ChangeRecurso (
					user,
					target,
					ConstantesRecursos.Equilibrio,
					-RecursoEquilibro.ReduceValue,
					1) { ShowDeltaLabel = false },
				true);

			ret.AddEffect (
				new ChangeRecurso (
					user,
					user,
					ConstantesRecursos.Equilibrio,
					-RecursoEquilibro.ReduceValue,
					1){ ShowDeltaLabel = false },
				true);

			var damage = baseDamage * HitDamageCalculator.Damage (
				             user, target,
				             ConstantesRecursos.Fuerza,
				             ConstantesRecursos.Destreza);

			ret.AddEffect (
				new ChangeRecurso (user, target, ConstantesRecursos.HP, -damage, 1));

			if (addDefaultCooldownTime)
				ret.AddEffect (
					new GenerateCooldownEffect (
						user,
						user,
						CalcularTiempoMelee (user)),
					true);

			return ret;
		}

		/// <summary>
		/// Calcula el tiempo que tarda una unidad en realizar un ataque melee
		/// </summary>
		public static float CalcularTiempoMelee (IUnidad user)
		{
			var dex = user.Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}
	}
}