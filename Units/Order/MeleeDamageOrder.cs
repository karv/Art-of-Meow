using Units.Recursos;

namespace Units.Order
{
	/// <summary>
	/// This order does all the efects on melee damage:
	/// doing damage and
	/// add a cooldown
	/// </summary>
	public class MeleeDamageOrder : ExecuteOrder
	{
		public IUnidad Target { get; set; }

		public float Damage { get; private set; }

		void doDamage (IUnidad unid)
		{
			var dex = Unidad.Recursos.GetRecurso (ConstantesRecursos.Destreza) as StatRecurso;
			dex.Valor *= 0.8f;

			if (Unidad.Equipo == Target.Equipo)
				return;
			var hp = Target.Recursos.GetRecurso (ConstantesRecursos.HP);
			Damage = Unidad.Recursos.ValorRecurso (ConstantesRecursos.DañoMelee) / 8;
			hp.Valor -= Damage;

		}

		public MeleeDamageOrder (IUnidad unidad,
		                         IUnidad target)
			: base (unidad, null)
		{
			Target = target;
			ActionOnFinish = doDamage;
		}
	}
}