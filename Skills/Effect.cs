using System;
using Skills;

namespace Skills
{
	public abstract class Effect : IEffect
	{
		protected abstract void WhenMiss ();

		void IEffect.WhenMiss ()
		{
			WhenMiss ();
			Executed?.Invoke (this, EffectResultEnum.Miss);
		}

		protected abstract void WhenHit ();

		void IEffect.WhenHit ()
		{
			WhenHit ();
			Executed?.Invoke (this, EffectResultEnum.Hit);
		}

		public abstract string DetailedInfo ();

		public IEffectAgent Agent { get; }

		public double Chance { get; set; }

		EffectResultEnum result;

		public EffectResultEnum Result
		{
			get
			{
				return result;
			}
			set
			{
				if (Result != EffectResultEnum.NotInstanced)
					throw new InvalidOperationException ("Result cannot be re-set");
				result = value;
			}
		}

		public event EventHandler<EffectResultEnum> Executed;

		protected Effect (IEffectAgent agent, double chance = 1)
		{
			Agent = agent;
			Chance = chance;
		}
	}
}	