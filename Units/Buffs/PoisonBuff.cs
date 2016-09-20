using Units.Recursos;

namespace Units.Buffs
{
	public class PoisonBuff : Buff
	{
		public override bool IsVisible { get { return true; } }

		public override string BaseTextureName { get { return "poison_buff"; } }

		public override string Nombre { get { return "Poison"; } }

		public override void Update (float gameTime)
		{
			var hp = Manager.HookedOn.Recursos.GetRecurso (ConstantesRecursos.HP);
			hp.Valor -= gameTime * 0.1f;
		}
	}
}