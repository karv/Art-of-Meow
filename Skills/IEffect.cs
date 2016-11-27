using Skills;
using System.Diagnostics;
using Helper;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using OpenTK.Platform.MacOS;
using System;

namespace Skills
{
	public class CollectionEffect : IEffect
	{
		readonly List<IEffect> _effects = new List<IEffect> ();

		#region Collection manipulation

		public void AddEffect (IEffect eff)
		{
			if (_effects.Contains (eff))
				throw new InvalidOperationException (
					"Duplicated effect in collection: ",
					eff);
			_effects.Add (eff);
		}

		public IEffect Effect (int i)
		{
			return _effects [i];
		}

		public bool RemoveEffect (IEffect eff)
		{
			return _effects.Remove (eff);
		}

		public void RemoveEffect (int i)
		{
			_effects.RemoveAt (i);
		}

		public int EffectCount { get { return _effects.Count; } }

		#endregion

		#region IEffect implementation

		public void ExecuteAll ()
		{
			for (int i = 0; i < _effects.Count; i++)
				_effects [i].Execute (true);
		}

		void IEffect.Execute (bool checkHit)
		{
			ExecuteAll ();
		}

		string IEffect.DetailedInfo ()
		{
			var sb = new StringBuilder ();
			foreach (var ef in _effects)
				sb.AppendLine (ef.DetailedInfo ());
			return sb.ToString ();
		}

		public IEffectAgent Agent { get; }

		public double Chance { get; }

		public EffectResultEnum IEffect.Result { get; set; }

		#endregion
	}

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