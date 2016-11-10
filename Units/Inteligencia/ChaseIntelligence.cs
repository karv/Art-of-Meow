using System.Linq;
using Cells;
using Cells.CellObjects;
using Units.Order;

namespace Units.Inteligencia
{
	/// <summary>
	/// Unidad's controllers. Chase and attack the human player
	/// </summary>
	public class ChaseIntelligence : IUnidadController
	{
		/// <summary>
		/// Gets the map grid.
		/// </summary>
		/// <value>The map grid.</value>
		public LogicGrid MapGrid { get { return ControlledUnidad.Grid; } }

		/// <summary>
		/// Gets the controlled unidad
		/// </summary>
		public readonly Unidad ControlledUnidad;

		Unidad Target;

		Unidad GetTarget ()
		{
			return MapGrid.Objects.OfType<Unidad> ().FirstOrDefault (isSelectableAsTarget);
		}

		bool isSelectableAsTarget (IGridObject obj)
		{
			var otro = obj as IUnidad;
			return otro != null && ControlledUnidad.IsEnemyOf (otro);
		}

		void TryUpdateTarget ()
		{
			if (Target == null)
				Target = GetTarget ();
		}

		void IUnidadController.DoAction ()
		{
			ControlledUnidad.assertIsIdleCheck ();
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Inteligencia.ChaseIntelligence"/> class.
		/// </summary>
		/// <param name="yo">The controlled unidad</param>
		public ChaseIntelligence (Unidad yo)
		{
			ControlledUnidad = yo;
		}
	}
}