using Skills;
using Units;
using Units.Buffs;

namespace Skills
{
	/// <summary>
	/// Effect: Add a new buff
	/// </summary>
	public class AddBuffEffect : ITargetEffect
	{
		/// <summary>
		/// Buff to add
		/// </summary>
		public readonly IBuff Buff;

		/// <summary>
		/// Target
		/// </summary>
		public readonly IUnidad Target;

		ITarget ITargetEffect.Target { get { return Target; } }

		#region IEffect implementation

		/// <summary>
		/// Ocurre cuando se falla
		/// </summary>
		public void WhenMiss ()
		{
		}

		/// <summary>
		/// Ocurre cuando s acierta
		/// </summary>
		public void WhenHit ()
		{
			Target.Buffs.Hook (Buff);
		}

		/// <summary>
		/// Gets the detailed information
		/// </summary>
		public string DetailedInfo ()
		{
			return string.Format ("{0}: +{1}", Chance, Buff.Nombre);
		}

		/// <summary>
		/// Quien causa el efecto
		/// </summary>
		public IEffectAgent Agent { get; }

		/// <summary>
		/// Chance of Hit, in [0,1]
		/// </summary>
		public double Chance { get; set; }

		/// <summary>
		/// Devuelve el resultado del hit-check
		/// </summary>
		/// <value>The result.</value>
		public EffectResultEnum Result { get; set; }

		#endregion

		/// <summary>
		/// </summary>
		public AddBuffEffect (IBuff buffType, IUnidad target)
		{
			if (target == null)
				throw new System.ArgumentNullException ("target");
			if (buffType == null)
				throw new System.ArgumentNullException ("buffType");
			Buff = buffType;
			Result = EffectResultEnum.NotInstanced;
			Chance = 1;
			Target = target;
		}
	}
}