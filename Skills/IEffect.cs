using Skills;
using System.Diagnostics;
using Helper;

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
		/// Runs the effect
		/// </summary>
		void Execute (bool checkHit);

		EffectResultEnum Result { get; set; }

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		string DetailedInfo ();
	}

	public static class EffectExt
	{
		public static bool CheckAndExecute (this IEffect eff)
		{
			eff.Result = (HitDamageCalculator.Hit (eff.Chance)) ? 
				EffectResultEnum.Hit : 
				EffectResultEnum.Miss;
			
			Debug.WriteLine (
				string.Format (
					"Resolving hit with probbility {0} : {1}",
					eff.Chance,
					eff.Result),
				"Effect");
			
			return eff.Result == EffectResultEnum.Hit;
		}
	}
}