using Skills;
using System.Collections.Generic;
using System.Text;
using System;

namespace Skills
{
	public class CollectionEffect : Effect, IEnumerable<IEffect>
	{
		readonly List<IEffect> _effects = new List<IEffect> ();
		readonly List<IEffect> _neverMissEffect = new List<IEffect> ();

		#region Enumeration

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

		public void AddEffect (IEffect eff, bool neverMiss = false)
		{
			List<IEffect> lst = neverMiss ? _neverMissEffect : _effects;
			if (lst.Contains (eff))
				throw new InvalidOperationException (
					"Duplicated effect in collection: " + eff);
			lst.Add (eff);
		}

		public IEffect Effect (int i)
		{
			// FIXME: Revisar _neverMissEffect
			return _effects [i];
		}

		public bool RemoveEffect (IEffect eff)
		{
			return _effects.Remove (eff);
		}

		public void RemoveEffect (int i)
		{
			// FIXME: Revisar _neverMissEffect
			_effects.RemoveAt (i);
		}

		public int EffectCount { get { return _effects.Count; } }

		#endregion

		#region IEffect implementation

		protected override void WhenHit ()
		{
			foreach (var ef in _neverMissEffect)
				ef.Execute ();
			foreach (var ef in _effects)
				ef.Execute ();
		}

		protected override void WhenMiss ()
		{
			foreach (var ef in _neverMissEffect)
				ef.Execute ();
			foreach (var ef in _effects)
				ef.WhenMiss ();
		}

		public void ExecuteAll ()
		{
			for (int i = 0; i < _effects.Count; i++)
				_effects [i].Execute ();
		}

		public override string DetailedInfo ()
		{
			var sb = new StringBuilder ();
			foreach (var ef in _effects)
				sb.AppendLine (ef.DetailedInfo ());
			return sb.ToString ();
		}

		#endregion

		public CollectionEffect (IEffectAgent agent, double chance = 1)
			: base (agent, chance)
		{
		}
	}
	
}