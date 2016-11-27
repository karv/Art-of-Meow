using Skills;
using System.Collections.Generic;
using System.Text;
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
	
}