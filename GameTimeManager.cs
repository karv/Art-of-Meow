using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cells;
using Cells.CellObjects;

namespace AoM
{
	/// <summary>
	/// Manager of in-game time
	/// </summary>
	public class GameTimeManager
	{
		/// <summary>
		/// Gets the map grid
		/// </summary>
		/// <value>The map grid.</value>
		Grid MapGrid { get; }

		/// <summary>
		/// Gets the entire collection of objects
		/// </summary>
		ICollection<IGridObject> Objects { get { return MapGrid.Objects; } }

		/// <summary>
		/// Gets the current object
		/// </summary>
		/// <value>The actual.</value>
		public IUpdateGridObject Actual { get; private set; }

		IEnumerable<IUpdateGridObject> UpdateGridObjects
		{
			get
			{
				return Objects.OfType<IUpdateGridObject> ().Where (z => z.Enabled);
			}
		}

		/// <summary>
		/// Devuelve el tiempo que ha pasado en este mapa
		/// </summary>
		public float TimePassed { get; private set; }

		/// <summary>
		/// Pass the time
		/// </summary>
		/// <param name="time">length of time to pass</param>
		public void PassTime (float time)
		{
			foreach (var ob in new List<IUpdateGridObject> (UpdateGridObjects))
				ob.PassTime (time);
			TimePassed += time;
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
			Debug.Assert (Actual.IsReady, "Actual was not ready");
			foreach (var actuador in UpdateGridObjects.Where (z => z.IsReady))
				actuador.Execute ();
			//Actual.Execute ();
			return passTime;
		}

		/// <summary>
		/// Gets the object with lower action time
		/// </summary>
		public IUpdateGridObject NextObject ()
		{
			var ret = UpdateGridObjects.Aggregate ((x, y) => x.NextActionTime < y.NextActionTime ? x : y);
			return ret;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AoM.GameTimeManager"/> class.
		/// </summary>
		/// <param name="grid">Grid.</param>
		public GameTimeManager (Grid grid)
		{
			MapGrid = grid;
		}
	}
}