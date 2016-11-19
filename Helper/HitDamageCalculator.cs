using System;
using Units;
using Units.Recursos;

namespace Helper
{
	/// <summary>
	/// Ayuda a calcular daño y hit%
	/// </summary>
	public static class HitDamageCalculator
	{
		readonly static Random _r = new Random ();

		/// <summary>
		/// Devuelve % de acierto dado los atributos relevantes
		/// </summary>
		/// <param name="attHit">Certeza</param>
		/// <param name="defEva">Evasión</param>
		/// <param name="equilibrio">Equilibrio</param>
		public static double GetPctHit (float attHit, float defEva, float equilibrio)
		{
			var rate = attHit * Math.Pow (equilibrio, 2) / defEva;
			return Math.Min (1, rate);
		}

		/// <summary>
		/// Devuelve el % de acuerto dado los nombres de los recursos relevantes
		/// </summary>
		/// <returns>The pct hit.</returns>
		/// <param name="att">Atacante</param>
		/// <param name="def">Defensor</param>
		/// <param name="attHitRecurso">Nombre del atributo de certeza</param>
		/// <param name="defEvaRecurso">Nombre del atributo de evasión</param>
		public static double GetPctHit (IUnidad att,
		                                IUnidad def,
		                                string attHitRecurso,
		                                string defEvaRecurso)
		{
			var attHit = att.Recursos.GetRecurso (attHitRecurso).Valor;
			var defEva = def.Recursos.GetRecurso (defEvaRecurso).Valor;
			return GetPctHit (
				attHit,
				defEva,
				att.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor);
		}

		public static float Damage (IUnidad att,
		                            IUnidad def,
		                            string attDmgRecurso,
		                            string defDefRecurso)
		{
			var attHit = att.Recursos.GetRecurso (attDmgRecurso).Valor;
			var defEva = def.Recursos.GetRecurso (defDefRecurso).Valor;

			// TODO: Calcular bien (esta es la misma fórmula que se usa en melee)
			return attHit * 0.35f;
		}

		/// <summary>
		/// Decide si un evento aleatorio es cierto
		/// </summary>
		/// <param name="pct">Probabilidad de ocurrencia del evento</param>
		public static bool Hit (double pct)
		{
			return _r.NextDouble () < pct;
		}
	}
}