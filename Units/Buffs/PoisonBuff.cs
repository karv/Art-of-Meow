using Units.Recursos;

namespace Units.Buffs
{
	/// <summary>
	/// Buff of poisoned state
	/// </summary>
	public class PoisonBuff : Buff
	{
		/// <summary>
		/// The buff is visible
		/// </summary>
		public override bool IsVisible { get { return true; } }

		/// <summary>
		/// Devuelve la textura a usar
		/// </summary>
		public override string BaseTextureName { get { return "poison_buff"; } }

		/// <summary>
		/// Name of the buff
		/// </summary>
		/// <value>The nombre.</value>
		public override string Nombre { get { return "Poison"; } }

		/// <summary>
		/// Updates the buff.
		/// Causes damage equal to 1/10 times the time passed
		/// </summary>
		/// <param name="gameTime">Time passed</param>
		public override void Update (float gameTime)
		{
			var hp = Manager.HookedOn.Recursos.GetRecurso (ConstantesRecursos.HP);
			hp.Valor -= gameTime * 0.1f;
		}
	}
}