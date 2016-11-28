using System;
using Skills;
using Units;
using Units.Order;

namespace Skills
{
	/// <summary>
	/// Efecto que ocasiona cooldown en una unidad
	/// </summary>
	public class GenerateCooldownEffect : Effect, ITargetEffect
	{
		#region IEffect implementation

		/// <summary>
		/// Se invoca cuando falla. No hace nada
		/// </summary>
		protected override void WhenMiss ()
		{
		}

		/// <summary>
		/// Se invoca cuando acierta.
		/// Agrega una orden primitiva de CoolDown
		/// </summary>
		protected override void WhenHit ()
		{
			
			var tg = Target as IUnidad;
			if (tg == null)
				throw new Exception ("IUnidad expected");

			tg.EnqueueOrder (new CooldownOrder (tg, CoolDownTime));
		}

		/// <summary>
		/// Detaileds the info.
		/// </summary>
		public override string DetailedInfo ()
		{
			return string.Format ("{1}-Cooldown: {0}", CoolDownTime, Chance);
		}

		/// <summary>
		/// Devuelve el tiempo de cooldown
		/// </summary>
		/// <value>The cool down time.</value>
		public float CoolDownTime { get; }

		#endregion

		#region ITargetEffect implementation

		/// <summary>
		/// Gets the target of the effect
		/// </summary>
		/// <value>The target.</value>
		public ITarget Target { get; }

		#endregion

		/// <summary>
		/// </summary>
		/// <param name="agent">Agente</param>
		/// <param name="tg">Quien estará en cooldown</param>
		/// <param name="cdLen">Tiempo de cooldown</param>
		/// <param name="chance">Chance.</param>
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