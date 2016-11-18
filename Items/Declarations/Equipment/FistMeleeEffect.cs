using Items.Declarations.Equipment;
using Units;
using Units.Order;
using Units.Recursos;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// The melee effect when the user has not equiped melee effect weapon
	/// </summary>
	public class FistMeleeEffect : IMeleeEffect
	{
		/// <summary>
		/// Causes damage on target
		/// </summary>
		/// <param name="user">User.</param>
		/// <param name="target">Target.</param>
		public void DoMeleeEffectOn (IUnidad user, IUnidad target)
		{
			var pct = Helper.HitDamageCalculator.GetPctHit (
				          user,
				          target,
				          ConstantesRecursos.Destreza,
				          ConstantesRecursos.Destreza);
			var eq = user.Recursos.GetRecurso (ConstantesRecursos.Equilibrio);
			eq.Valor /= 2;
			if (Helper.HitDamageCalculator.Hit (pct))
			{
				var damage = user.Recursos.ValorRecurso (ConstantesRecursos.Fuerza) / 8;
				user.EnqueueOrder (new MeleeDamageOrder (user, target, damage));
			}
			user.EnqueueOrder (new CooldownOrder (user, calcularTiempoMelee (user)));
		}

		static float calcularTiempoMelee (IUnidad user)
		{
			var dex = user.Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}
	}
}