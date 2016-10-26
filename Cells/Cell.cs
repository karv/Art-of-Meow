using System.Collections.Generic;
using Cells.CellObjects;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Units;

namespace Cells
{
	/// <summary>
	/// A state of a grid generated at some point.
	/// </summary>
	/// <remarks>Modify this class won't change the <see cref="Grid"/></remarks>
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

		/// <summary>
		/// Determines if this cell contains a <see cref="IGridObject"/>
		/// </summary>
		/// <param name="obj">Object to determine if it is contained</param>
		public bool Contains (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			
			foreach (var x in Objects)
				if (obj.Equals (x))
					return true;

			return false;
		}


		/// <summary>
		/// Gets the first objects satisfacing a predicate
		/// </summary>
		/// <returns>An object which satisface a predicate. <c>null</c> if it does not exist</returns>
		/// <param name="pred">Predicate</param>
		public IGridObject ExistsReturn (Predicate<IGridObject> pred)
		{
			return Objects.FirstOrDefault (z => pred (z));
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

		public override string ToString ()
		{
			return string.Format ("[Cell: Objects={0}]", Objects);
		}

		public Cell (IEnumerable<IGridObject> objs)
		{
			Objects = new List<IGridObject> (objs);
		}
	}
}