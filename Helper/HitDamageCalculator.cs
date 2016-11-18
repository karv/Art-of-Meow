using System;
using Units;
using Units.Recursos;

namespace Helper
{
	public static class HitDamageCalculator
	{
		static Random _r = new Random ();

		public static double GetPctHit (float attHit, float defEva, float equilibrio)
		{
			var rate = attHit * Math.Pow (equilibrio, 2) / defEva;
			return Math.Min (1, rate); // TODO
		}

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

		public static bool Hit (double pct)
		{
			return _r.NextDouble () < pct;
		}
	}
}