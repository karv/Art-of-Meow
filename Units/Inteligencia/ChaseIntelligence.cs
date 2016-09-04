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
			return MapGrid.Objects.FirstOrDefault (isSelectableAsTarget) as Unidad;
		}

		static bool isSelectableAsTarget (IGridObject obj)
		{
			return (obj as Unidad)?.Inteligencia is HumanIntelligence;
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
			MapGrid.MoveCellObject (Yo, dir);
		}

		public ChaseIntelligence (Unidad yo)
		{
			Yo = yo;
		}
	}
}