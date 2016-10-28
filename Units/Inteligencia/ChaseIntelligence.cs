using System.Linq;
using Cells;
using Cells.CellObjects;

namespace Units.Inteligencia
{
	/// <summary>
	/// Unidad's controllers. Chase and attack the human player
	/// </summary>
	public class ChaseIntelligence : IIntelligence
	{
		/// <summary>
		/// Gets the map grid.
		/// </summary>
		/// <value>The map grid.</value>
		public Grid MapGrid { get { return ControlledUnidad.MapGrid; } }

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
			var otro = obj as Unidad;
			if (otro == null)
				return false;
			return otro.Equipo != ControlledUnidad.Equipo;
		}

		void TryUpdateTarget ()
		{
			if (Target == null)
				Target = GetTarget ();
		}

		void IIntelligence.DoAction ()
		{
			TryUpdateTarget ();
			var dir = ControlledUnidad.Location.GetDirectionTo (Target.Location);
			if (dir == MovementDirectionEnum.NoMov)
				return;
			ControlledUnidad.MoveOrMelee (dir);
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