using System.Linq;
using Cells;
using System;
using Cells.CellObjects;

namespace Units.Inteligencia
{
	public class ChaseIntelligence  : IIntelligence
	{
		public Grid MapGrid { get { return Yo.MapGrid; } }

		public readonly Unidad Yo;

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
			return otro.Equipo != Yo.Equipo;
		}

		void TryUpdateTarget ()
		{
			if (Target == null)
				Target = GetTarget ();
		}

		void IIntelligence.DoAction ()
		{
			Yo.NextActionTime = TimeSpan.FromMinutes (2);
			TryUpdateTarget ();
			var dir = Yo.Location.GetDirectionTo (Target.Location);
			if (dir == MovementDirectionEnum.NoMov)
				return;
			Yo.MoveOrMelee (dir);
		}

		public ChaseIntelligence (Unidad yo)
		{
			Yo = yo;
		}
	}
}