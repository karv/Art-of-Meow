using System.Collections.Generic;
using Cells.CellObjects;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Units;

namespace Cells
{
	public class Cell
	{
		readonly List<IGridObject> Objects;

		/// <summary>
		/// Devuelve el peso de movimiento
		/// </summary>
		public float PesoMovimiento ()
		{
			var ret = 1f;
			foreach (var movCell in Objects.OfType<IMovementGridObject> ())
				ret += movCell.CoefMovement;
			return ret;
		}

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

		public IUnidad GetUnidadHere ()
		{
			return ExistsReturn (z => z is IUnidad) as IUnidad;
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