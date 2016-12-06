using System;
using System.Collections.Generic;
using System.Text;
using Skills;

namespace Skills
{
	/// <summary>
	/// Una colección de efectos que actúan como uno
	/// </summary>
	public class CollectionEffect : Effect, IEnumerable<IEffect>
	{
		readonly List<IEffect> _effects = new List<IEffect> ();
		readonly List<IEffect> _neverMissEffect = new List<IEffect> ();

		#region Enumeration

		/// <summary>
		/// Enumera los efectos
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<IEffect> GetEnumerator ()
		{
			foreach (var ef in _neverMissEffect)
				yield return ef;
			foreach (var ef in _effects)
				yield return ef;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion

		#region Collection manipulation

		/// <summary>
		/// Añade un efecto
		/// </summary>
		/// <param name="eff">Efecto</param>
		/// <param name="neverMiss">If set to <c>true</c>, the effect never miss.</param>
		public void AddEffect (IEffect eff, bool neverMiss = false)
		{
			List<IEffect> lst = neverMiss ? _neverMissEffect : _effects;
			if (lst.Contains (eff))
				throw new InvalidOperationException (
					"Duplicated effect in collection: " + eff);
			lst.Add (eff);
		}

		/// <summary>
		/// Gets the effect with a given index
		/// </summary>
		/// <param name="i">The index.</param>
		public IEffect Effect (int i)
		{
			return i < _effects.Count ? _effects [i] : _neverMissEffect [i - _effects.Count];
		}

		/// <summary>
		/// Elimina un efecto
		/// </summary>
		/// <param name="eff">Efecto</param>
		public bool RemoveEffect (IEffect eff)
		{
			return _effects.Remove (eff);
		}

		/// <summary>
		/// Elimina un efecto
		/// </summary>
		/// <param name="i">Índice del efecto</param>
		public void RemoveEffect (int i)
		{
			if (i < _effects.Count)
				_effects.RemoveAt (i);
			else
				_neverMissEffect.RemoveAt (i - _effects.Count);
		}

		/// <summary>
		/// Devuelve el número de efectos existentes
		/// </summary>
		/// <value>The effect count.</value>
		public int EffectCount { get { return _effects.Count + _neverMissEffect.Count; } }

		#endregion

		#region IEffect implementation

		/// <summary>
		/// Se invoca cuando acierta,
		/// se ejecutan los efectos debidos
		/// </summary>
		protected override void WhenHit ()
		{
			foreach (var ef in _neverMissEffect)
				ef.Execute ();
			foreach (var ef in _effects)
				ef.Execute ();
		}

		/// <summary>
		/// Se invoca cuando falla.
		/// Ejecuta los efectos que nunca fallan y falla los otros
		/// </summary>
		protected override void WhenMiss ()
		{
			foreach (var ef in _neverMissEffect)
				ef.Execute ();
			foreach (var ef in _effects)
				ef.WhenMiss ();
		}

		/// <summary>
		/// Ejecuta todos los efectos
		/// </summary>
		[Obsolete]
		public void ExecuteAll ()
		{
			for (int i = 0; i < _effects.Count; i++)
				_effects [i].Execute ();
		}

		/// <summary>
		/// Devuelve la información detallada en forma de lista
		/// </summary>
		public override string DetailedInfo ()
		{
			var sb = new StringBuilder ();
			foreach (var ef in _effects)
				sb.AppendLine (ef.DetailedInfo ());
			return sb.ToString ();
		}

		#endregion

		/// <summary>
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="chance">Chance.</param>
		public CollectionEffect (IEffectAgent agent, double chance = 1)
			: base (agent, chance)
		{
		}
	}
}