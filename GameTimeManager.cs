﻿using System;
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
		LogicGrid MapGrid { get; }

		/// <summary>
		/// Gets the entire collection of objects
		/// </summary>
		IEnumerable<IGridObject> Objects { get { return MapGrid.Objects; } }

		IUpdateGridObject _actual;

		/// <summary>
		/// Gets the current object
		/// </summary>
		/// <value>The actual.</value>
		public IUpdateGridObject Actual
		{
			get
			{
				return _actual;
			}
			private set
			{
				if (Actual == value)
					return;

				LostTurn?.Invoke (this, Actual);
				_actual = value;
				GotTurn?.Invoke (this, Actual);
			}
		}

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
			if (time == 0)
				return;
			foreach (var ob in new List<IUpdateGridObject> (UpdateGridObjects))
				ob.PassTime (time);
			TimePassed += time;
			AfterTimePassed?.Invoke (this, time);
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
			assertReady ();
			foreach (var actuador in UpdateGridObjects.Where (z => z.IsReady))
				actuador.Execute ();
			//Actual.Execute ();
			return passTime;
		}

		[Conditional ("DEBUG")]
		void assertReady ()
		{
			if (!Actual.IsReady)
				throw new Exception ();
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
		/// Occurs when a new object gets the turn
		/// </summary>
		public event EventHandler<IUpdateGridObject> GotTurn;

		/// <summary>
		/// Occurs when the current object loses the turn
		/// </summary>
		public event EventHandler<IUpdateGridObject> LostTurn;

		/// <summary>
		/// Occurs when time > 0 passed.
		/// </summary>
		public event EventHandler<float> AfterTimePassed;

		/// <summary>
		/// Initializes a new instance of the <see cref="AoM.GameTimeManager"/> class.
		/// </summary>
		/// <param name="grid">Grid.</param>
		public GameTimeManager (LogicGrid grid)
		{
			MapGrid = grid;
		}
	}
}