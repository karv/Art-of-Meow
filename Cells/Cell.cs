using System.Collections.Generic;
using Cells.CellObjects;
using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Cells
{
	public class Cell
	{
		readonly List<ICellObject> Objects;

		public bool Contains (ICellObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			
			foreach (var x in Objects)
				if (obj.Equals (x))
					return true;

			return false;
		}

		/// <summary>
		/// Determina si esta celda evita que un objeto pueda entrar.
		/// </summary>
		public bool Collision (ICellObject collObj)
		{
			return Objects.Any (z => z.Collision (collObj));
		}

		public Cell (Grid grid, Point location)
		{
			Objects = new List<ICellObject> ();
			foreach (var x in grid.Objects)
				if (x.Location == location)
					Objects.Add (x);
		}

		public Cell (IEnumerable<ICellObject> objs)
		{
			Objects = new List<ICellObject> (objs);
		}
	}
}