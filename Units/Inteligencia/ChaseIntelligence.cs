using System.Linq;
using Units.Order;

namespace Units.Inteligencia
{
	/// <summary>
	/// Unidad's controllers. Chase and attack the human player
	/// </summary>
	public class ChaseIntelligence : AI
	{
		Unidad Target;

		Unidad GetTarget ()
		{
			return MapGrid.Objects.OfType<Unidad> ().FirstOrDefault (IsSelectableAsTarget);
		}

		public override object Clone ()
		{
			return new ChaseIntelligence ();
		}

		void TryUpdateTarget ()
		{
			if (Target == null)
				Target = GetTarget ();
		}

		protected override void DoAction ()
		{
			TryUpdateTarget ();
			if (Target == null)
				return; // Si no encuentra enemigo, debe ser porque la instancia de mapa se está desechando
			var dir = ControlledUnidad.Location.GetDirectionTo (Target.Location);
			if (dir == MovementDirectionEnum.NoMov)
				return;
			if (!ControlledUnidad.MoveOrMelee (dir))
			{
				// No se puede moverse ni atacar hacia acá.
				// Solamente voy a esperar un poco
				ControlledUnidad.EnqueueOrder (new CooldownOrder (ControlledUnidad, 0.1f));
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Inteligencia.ChaseIntelligence"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Inteligencia.ChaseIntelligence"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[ChaseIntelligence: Yo={0}, Target={1}]",
				ControlledUnidad.Nombre,
				Target.Nombre);
		}
	}
}