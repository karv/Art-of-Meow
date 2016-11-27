using System;
using System.Diagnostics;
using Helper;
using Skills;

namespace Skills
{

	/// <summary>
	/// Un efecto de un objeto.
	/// </summary>
	public interface IEffect
	{
		/// <summary>
		/// Quien causa el efecto
		/// </summary>
		IEffectAgent Agent { get; }

		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		double Chance { get; }

		/// <summary>
		/// Ocurre cuando se falla
		/// </summary>
		void WhenMiss ();

		/// <summary>
		/// Ocurre cuando s acierta
		/// </summary>
		void WhenHit ();

		/// <summary>
		/// Devuelve el resultado del hit-check
		/// </summary>
		EffectResultEnum Result { get; set; }

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		string DetailedInfo ();
	}

	/// <summary>
	/// Extensiones de efectos de skill
	/// </summary>
	public static class EffectExt
	{
		/// <summary>
		/// Hace un hit-checkM de ser positivo ejecuta el efecto
		/// </summary>
		public static bool Execute (this IEffect eff)
		{
			eff.Result = (HitDamageCalculator.Hit (eff.Chance)) ? 
				EffectResultEnum.Hit : 
				EffectResultEnum.Miss;
			
			Debug.WriteLine (
				string.Format (
					"Resolving hit with probability {0} : {1}",
					eff.Chance,
					eff.Result),
				"Effect");
			
			var ret = eff.Result == EffectResultEnum.Hit;
			if (ret)
				eff.WhenHit ();
			else
				eff.WhenMiss ();

			return ret;
		}
	}
}	