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
		/// <param name = "baseHit">The probability of hit when hit is equal to defense</param>
		public static double GetPctHit (float attHit,
		                                float defEva,
		                                double baseHit)
		{
			double pctRet;
			var diff = attHit - defEva;
			if (diff == 0)
				pctRet = baseHit;
			else if (diff < 0)
				pctRet = Math.Pow (baseHit, 1 - diff);
			else if (diff < 1)
				pctRet = baseHit + (1 - baseHit) * diff / 2d;
			else
			{
				var pow2 = Math.Pow (2, -diff);
				pctRet = (pow2 - 1 + baseHit) / (pow2);
			}
			pctRet = Math.Min (Math.Max (pctRet, 0), 1);
			return pctRet;
		}

		/// <summary>
		/// Devuelve el % de acuerto dado los nombres de los recursos relevantes
		/// </summary>
		/// <returns>The pct hit.</returns>
		/// <param name="att">Atacante</param>
		/// <param name="def">Defensor</param>
		/// <param name="attHitRecurso">Nombre del atributo de certeza</param>
		/// <param name="defEvaRecurso">Nombre del atributo de evasión</param>
		/// <param name = "baseHit">The probability of hit when hit is equal to defense</param>
		public static double GetPctHit (IUnidad att,
		                                IUnidad def,
		                                string attHitRecurso,
		                                string defEvaRecurso,
		                                double baseHit)
		{
			if (def == null)
				throw new ArgumentNullException ("def");
			if (att == null)
				throw new ArgumentNullException ("att");

			var attHit = att.Recursos.GetRecurso (attHitRecurso).Valor * att.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor;
			var defEva = def.Recursos.GetRecurso (defEvaRecurso).Valor * def.Recursos.GetRecurso (ConstantesRecursos.Equilibrio).Valor;
			return GetPctHit (
				attHit,
				defEva,
				baseHit);
		}

		/// <summary>
		/// Devuelve el daño que se debe de producir en habilidades con un sólo atributo de ataque y de defensa
		/// </summary>
		/// <param name="att">Atacante</param>
		/// <param name="def">Defensor</param>
		/// <param name="attDmgRecurso">Nombre del recurso de habilidad de ataque</param>
		/// <param name="defDefRecurso">Nombre del recurso de habilidad de defensa</param>
		public static float Damage (IUnidad att,
		                            IUnidad def,
		                            string attDmgRecurso,
		                            string defDefRecurso)
		{
			if (def == null)
				throw new ArgumentNullException ("def");
			if (att == null)
				throw new ArgumentNullException ("att");
			
			var attStr = att.Recursos.GetRecurso (attDmgRecurso).Valor;
			var defAC = def.Recursos.GetRecurso (defDefRecurso).Valor;

			var diffStat = Math.Max (0, 2 * attStr - defAC);
			return diffStat * 0.35f;
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