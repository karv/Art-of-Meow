using Skills;
using Units;
using Units.Buffs;

namespace Skills
{
	public class AddBuffEffect : ITargetEffect
	{
		public IBuff Buff;

		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		#region IEffect implementation

		public void WhenMiss ()
		{
		}

		public void WhenHit ()
		{
			Target.Buffs.Hook (Buff);
		}

		public string DetailedInfo ()
		{
			return string.Format ("{0}: +{1}", Chance, Buff.Nombre);
		}

		public IEffectAgent Agent { get; }

		public double Chance { get; set; }

		public EffectResultEnum Result { get; set; }

		#endregion

		public AddBuffEffect (IBuff buffType, IUnidad target)
		{
			Buff = buffType;
			Result = EffectResultEnum.NotInstanced;
			Chance = 1;
			Target = target;
		}
	}
}