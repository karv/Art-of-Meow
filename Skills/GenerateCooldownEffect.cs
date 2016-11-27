using System;
using Skills;
using Units;
using Units.Order;

namespace Skills
{
	public class GenerateCooldownEffect : Effect, ITargetEffect
	{
		#region IEffect implementation

		protected override void WhenMiss ()
		{
		}

		protected override void WhenHit ()
		{
			
			var tg = Target as IUnidad;
			if (tg == null)
				throw new Exception ("IUnidad expected");

			tg.EnqueueOrder (new CooldownOrder (tg, CoolDownTime));
		}

		public override string DetailedInfo ()
		{
			return string.Format ("{1}-Cooldown: {0}", CoolDownTime, Chance);
		}

		public float CoolDownTime { get; }

		public IEffectAgent Agent { get; }

		#endregion

		#region ITargetEffect implementation

		public ITarget Target { get; }

		#endregion

		public GenerateCooldownEffect (IEffectAgent agent,
		                               ITarget tg,
		                               float cdLen,
		                               double chance = 1)
			: base (agent,
			        chance)
		{
			Target = tg;
			CoolDownTime = cdLen;
		}
	}
}