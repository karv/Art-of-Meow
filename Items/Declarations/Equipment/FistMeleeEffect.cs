using Items.Declarations.Equipment;
using Units.Order;
using Units.Recursos;
using Units;

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
			var damage = user.Recursos.ValorRecurso (ConstantesRecursos.Fuerza) / 8;
			user.EnqueueOrder (new MeleeDamageOrder (user, target, damage));
			user.EnqueueOrder (new CooldownOrder (user, calcularTiempoMelee (user)));
		}

		static float calcularTiempoMelee (IUnidad user)
		{
			var dex = user.Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}
	}
}