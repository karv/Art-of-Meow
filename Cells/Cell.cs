using System.Collections.Generic;
using Cells.CellObjects;
using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cells
{
	public class Cell
	{
		readonly List<IGridObject> Objects;

		public bool Contains (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			
			foreach (var x in Objects)
				if (obj.Equals (x))
					return true;

			return false;
		}

		public IGridObject ExistsReturn (Predicate<IGridObject> pred)
		{
			foreach (var x in Objects)
				if (pred (x))
					return x;

			return null;
		}

		/// <summary>
		/// Determina si esta celda evita que un objeto pueda entrar.
		/// </summary>
		public bool Collision (IGridObject collObj)
		{
			return Objects.Any (z => z.Collision (collObj) || collObj.Collision (z));
		}

		public Cell (Grid grid, Point location)
		{
			Objects = new List<IGridObject> ();
			foreach (var x in grid.Objects)
				if (x.Location == location)
					Objects.Add (x);
		}

		public Cell (IEnumerable<IGridObject> objs)
		{
			Objects = new List<IGridObject> (objs);
		}
	}
}