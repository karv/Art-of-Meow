using Skills;
using Units;
using Units.Recursos;
using System.Diagnostics;

namespace Skills
{
	/// <summary>
	/// Un efecto de cambio de recurso en un target
	/// </summary>
	public class ChangeRecurso : ITargetEffect
	{
		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		/// <value>The chance.</value>
		public double Chance { get; set; }

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		public string DetailedInfo ()
		{
			if (Parámetro == null)
				return string.Format (
					"{3}: {0}'s {1} changed by {2}.",
					Target,
					TargetRecurso.NombreLargo,
					DeltaValor,
					Chance);
			return string.Format (
				"{4}: {0}'s {1}'s {2} changed by {3}.",
				Target,
				TargetRecurso.NombreLargo,
				Parámetro.NombreÚnico,
				DeltaValor,
				Chance);
		}

		/// <summary>
		/// Runs the effect
		/// </summary>
		public void Execute ()
		{
			if (Parámetro == null)
				// Efecto es en recurso
				TargetRecurso.Valor += DeltaValor;
			else
				// Efecto es en parámetro
				Parámetro.Valor += DeltaValor;

			if (TargetRecurso is RecursoHP && TargetRecurso.Valor == 0)
				OnKill ();
		}

		/// <summary>
		/// Se invoca cuando este efecto asesina a una unidad
		/// </summary>
		protected void OnKill ()
		{
			var expGetter = Agent as IUnidad;
			if (expGetter != null)
			{
				var exp = Target.GetExperienceValue ();
				expGetter.Exp.ExperienciaAcumulada += exp;
				Debug.WriteLine (string.Format (
					"{0} kills {1}.\nReceving {2} exp",
					Agent,
					Target,
					exp), "Experience");
			}
		}

		/// <summary>
		/// El recurso que es afectado.
		/// El valor de este recurso no es afectado, al menos que <see cref="Parámetro"/> sea <c>null</c>
		/// </summary>
		public IRecurso TargetRecurso { get; }

		/// <summary>
		/// Devuelve el parámetro del recurso afectado; 
		/// si es <c>null</c>, se afecta directamente a <see cref="TargetRecurso"/>
		/// </summary>
		/// <value>The parámetro.</value>
		public IParámetroRecurso Parámetro { get; }

		/// <summary>
		/// Devuelve la unidad que es afectada
		/// </summary>
		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		/// <summary>
		/// Devuelve quien causa el efecto.
		/// </summary>
		public IEffectAgent Agent { get; }

		/// <summary>
		/// Devuelve la diferencia de valor que causa el recurso
		/// </summary>
		public float DeltaValor { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.ChangeRecurso"/> class.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="target">Target.</param>
		/// <param name="recNombre">Nombre del recurso</param>
		/// <param name="recParámetro">Nombre del parámetro</param>
		/// <param name="deltaValor">Cambio de valor</param>
		public ChangeRecurso (IEffectAgent agent,
		                      IUnidad target,
		                      string recNombre,
		                      string recParámetro, 
		                      float deltaValor)
		{
			Agent = agent;
			Target = target;
			TargetRecurso = target.Recursos.GetRecurso (recNombre);
			Parámetro = TargetRecurso.ValorParámetro (recParámetro);
			DeltaValor = deltaValor;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.ChangeRecurso"/> class.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="target">Target.</param>
		/// <param name="recNombre">Nombre del recurso</param>
		/// <param name="deltaValor">Cambio de valor</param>
		public ChangeRecurso (IEffectAgent agent,
		                      IUnidad target,
		                      string recNombre, 
		                      float deltaValor)
		{
			Agent = agent;
			Target = target;
			TargetRecurso = target.Recursos.GetRecurso (recNombre);
			DeltaValor = deltaValor;
		}
	}
}