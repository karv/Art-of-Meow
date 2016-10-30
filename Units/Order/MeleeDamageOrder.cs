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
		/// <summary>
		/// Gets or sets the target for this order
		/// </summary>
		/// <value>The target.</value>
		public IUnidad Target { get; set; }

		/// <summary>
		/// Gets the damage
		/// </summary>
		/// <value>The damage.</value>
		public float Damage { get; }

		void doDamage (IUnidad unid)
		{
			var dex = Unidad.Recursos.GetRecurso (ConstantesRecursos.Destreza) as StatRecurso;
			dex.Valor *= 0.8f;

			if (Unidad.Team == Target.Team)
				return;
			var hp = Target.Recursos.GetRecurso (ConstantesRecursos.HP);
			hp.Valor -= Damage;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Order.MeleeDamageOrder"/> class.
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		/// <param name="target">Target.</param>
		/// <param name="damage">Damage caused</param>
		public MeleeDamageOrder (IUnidad unidad,
		                         IUnidad target, float damage)
			: base (unidad, null)
		{
			Target = target;
			ActionOnFinish = doDamage;
			Damage = damage;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Order.MeleeDamageOrder"/> class.
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		/// <param name="target">Target.</param>
		public MeleeDamageOrder (IUnidad unidad,
		                         IUnidad target)
			: this (unidad, target, 0)
		{
			Damage = Unidad.Recursos.ValorRecurso (ConstantesRecursos.DañoMelee) / 8;
		}
		
	}
}