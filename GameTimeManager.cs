using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.CellObjects;
using System.Diagnostics;

namespace Art_of_Meow
{
	public class GameTimeManager
	{
		Grid MapGrid { get; }

		ICollection<IGridObject> Objects { get { return MapGrid.Objects; } }

		public IUpdateGridObject Actual { get; private set; }

		IEnumerable<IUpdateGridObject> UpdateGridObjects
		{
			get
			{
				return Objects.OfType<IUpdateGridObject> ();
			}
		}

		public void PassTime (TimeSpan time)
		{
			foreach (var ob in UpdateGridObjects)
				ob.PassTime (time);
		}

		public TimeSpan ExecuteNext ()
		{
			Actual = NextObject ();
			if (Actual == null)
				throw new Exception ("No existe objeto Update.");
			PassTime (Actual.NextActionTime);
			if (Actual.NextActionTime != TimeSpan.Zero)
			{
				Debug.WriteLine (
					"Ejecutando objeto con tiempo de espera positivo",
					"IUpdateGridObject");
			}
			Actual.Execute ();
			return Actual.NextActionTime;
		}

		public IUpdateGridObject NextObject ()
		{
			IUpdateGridObject ret = null;
			foreach (var ob in UpdateGridObjects)
				if (ret == null || ret.NextActionTime > ob.NextActionTime)
					ret = ob;
			return ret;
		}

		public GameTimeManager (Grid grid)
		{
			MapGrid = grid;
		}
	}
}