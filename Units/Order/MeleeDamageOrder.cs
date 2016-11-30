using System;
using Helper;
using Skills;
using Units.Recursos;

namespace Units.Order
{
	/// <summary>
	/// This order does all the efects on melee damage:
	/// doing damage and
	/// add a cooldown
	/// </summary>
	[ObsoleteAttribute]
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

			// Asignar exp
			unid.Exp.AddAssignation (ConstantesRecursos.Destreza, "base", 0.1f);
			unid.Exp.AddAssignation (ConstantesRecursos.CertezaMelee, "base", 0.05f);
			Target.Exp.AddAssignation (ConstantesRecursos.EvasiónMelee, "base", 0.05f);

			var ef = new ChangeRecurso (unid, Target, ConstantesRecursos.HP, -Damage);
			const double baseHit = 0.9;
			ef.Chance = HitDamageCalculator.GetPctHit (
				unid,
				Target,
				ConstantesRecursos.CertezaMelee,
				ConstantesRecursos.EvasiónMelee,
				baseHit);

			ef.Execute ();
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
	}
}