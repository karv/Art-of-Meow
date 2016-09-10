using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Microsoft.Xna.Framework.Graphics;
using Units;
using Units.Inteligencia;

namespace AoM
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

		public void PassTime (float time)
		{
			foreach (var ob in UpdateGridObjects)
				ob.PassTime (time);
		}

		/// <summary>
		/// Runs the game until the next action
		/// </summary>
		/// <returns>The passed internal time.</returns>
		public float ExecuteNext ()
		{
			Actual = NextObject ();
			if (Actual == null)
				throw new Exception ("No existe objeto Update.");
			var passTime = Actual.NextActionTime;
			#if DEBUG
			if (passTime < 0)
				throw new Exception ("passTime < 0");
			#endif
			PassTime (passTime);
			Debug.WriteLineIf (Actual.NextActionTime != 0,
				"Ejecutando objeto con tiempo de espera positivo",
				"IUpdateGridObject");
			Actual.Execute ();
			return passTime;
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