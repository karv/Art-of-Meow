using System;
using Skills;

namespace Skills
{
	/// <summary>
	/// Implementación de eventos y campos en IEffect
	/// </summary>
	public abstract class Effect : IEffect
	{
		/// <summary>
		/// Se invoca cuando falla
		/// </summary>
		protected abstract void WhenMiss ();

		void IEffect.WhenMiss ()
		{
			WhenMiss ();
			Executed?.Invoke (this, EffectResultEnum.Miss);
		}

		/// <summary>
		/// Se invoca cuando acierta
		/// </summary>
		protected abstract void WhenHit ();

		void IEffect.WhenHit ()
		{
			WhenHit ();
			Executed?.Invoke (this, EffectResultEnum.Hit);
		}

		/// <summary>
		/// Una descripción enlistada de este efecto
		/// </summary>
		public abstract string DetailedInfo ();

		/// <summary>
		/// Quien causa el efecto
		/// </summary>
		public IEffectAgent Agent { get; }

		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		/// <value>The chance.</value>
		public double Chance { get; set; }

		EffectResultEnum result;

		/// <summary>
		/// Devuelve el resultado del hit-check
		/// </summary>
		/// <value>The result.</value>
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

		/// <summary>
		/// Ocurre cuando se ejecuta, da como parámetro el resultado del hit-check
		/// </summary>
		public event EventHandler<EffectResultEnum> Executed;

		/// <summary>
		/// </summary>
		/// <param name="agent">Agente</param>
		/// <param name="chance">Probabilidad de acierto</param>
		protected Effect (IEffectAgent agent, double chance = 1)
		{
			Agent = agent;
			Chance = chance;
		}
	}
}	