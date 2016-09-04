using System.Linq;
using Cells;
using System;

namespace Units
{
	public class ChaseIntelligence  : IIntelligence
	{
		public ChaseIntelligence (Unidad yo)
		{
			Yo = yo;
		}

		public Grid MapGrid { get { return Yo.MapGrid; } }

		public readonly Unidad Yo;

		public void DoAction ()
		{
			Yo.NextActionTime = TimeSpan.FromMinutes (2);
			var target = MapGrid.Objects.FirstOrDefault (z => z is Unidad);
			var dir = Yo.Location.GetDirectionTo (target.Location);
			if (dir == MovementDirectionEnum.NoMov)
				return;
			MapGrid.MoveCellObject (Yo, dir);
		}
	}
}