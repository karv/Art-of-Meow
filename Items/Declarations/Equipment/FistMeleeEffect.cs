using Items.Declarations.Equipment;
using Units.Order;
using Units.Recursos;
using Units;

namespace Items.Declarations.Equipment
{
	
	public class FistMeleeEffect : IMeleeEffect
	{
		IUnidad Owner { get; }

		public void DoMeleeOn (IUnidad target)
		{
			Owner.EnqueueOrder (new MeleeDamageOrder (Owner, target));
			Owner.EnqueueOrder (new CooldownOrder (Owner, calcularTiempoMelee ()));
		}

		float calcularTiempoMelee ()
		{
			var dex = Owner.Recursos.ValorRecurso (ConstantesRecursos.Destreza);
			return 1 / dex;
		}

		public FistMeleeEffect (IUnidad owner)
		{
			Owner = owner;
		}
	}
}