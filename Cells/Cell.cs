using System;
using System.Collections.Generic;
using System.Linq;
using Cells.CellObjects;
using Microsoft.Xna.Framework;
using Units;
using Moggle.Controles;

namespace Cells
{
	/// <summary>
	/// A state of a grid generated at some point.
	/// </summary>
	/// <remarks>Modify this class won't change the <see cref="LogicGrid"/></remarks>
	public class Cell : IDibujable
	{
		/// <summary>
		/// Devuelve un valor determinando si este grid bloquea visibilidad.
		/// </summary>
		public bool BlocksVisibility ()
		{
			return Objects.OfType<GridWall> ().Any ();
		}

		public Point Location { get; }

		/// <summary>
		/// Gets the list of <see cref="IGridObject"/> in this cell
		/// </summary>
		public List<IGridObject> Objects { get; }

		public bool Remove (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			if (Location != obj.Location)
				throw new InvalidOperationException ();
			return Objects.Remove (obj);
		}

		public void Add (IGridObject obj)
		{
			if (obj == null)
				throw new ArgumentNullException ("obj");
			if (Location != obj.Location)
				throw new InvalidOperationException ();
			Objects.Add (obj);
		}

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

		public void Draw (Microsoft.Xna.Framework.Graphics.SpriteBatch bat,
		                  Rectangle rect)
		{
			foreach (var obj in Objects)
				obj.Draw (bat, rect);
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

		/// <summary>
		/// Gets the <see cref="IUnidad"/> in this cell if any. <c>null</c> otherwise
		/// </summary>
		public IUnidad GetUnidadHere ()
		{
			return ExistsReturn (z => z is IUnidad) as IUnidad;
		}

		/// <summary>
		/// Devuelve la unidad vida que está aquí (o null)
		/// </summary>
		public IUnidad GetAliveUnidadHere ()
		{
			return Objects.OfType<IUnidad> ().FirstOrDefault (z => z.Habilitado);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Cells.Cell"/>.
		/// </summary>
		public override string ToString ()
		{
			return string.Format ("[Cell: Objects={0}]", Objects);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.Cell"/> class.
		/// </summary>
		/// <param name="grid">Grid of this <c>Cell</c></param>
		/// <param name="location">Grid-wise coordinates of this Cell</param>
		public Cell (Point location)
		{
			Location = location;
			Objects = new List<IGridObject> ();
		}
	}
}