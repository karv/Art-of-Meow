using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Cells.CellObjects;
using System;
using System.Linq;

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

		public ICellObject ExistsReturn (Predicate<ICellObject> pred)
		{
			foreach (var x in Objects)
				if (pred (x))
					return x;

			return null;
		}

		/// <summary>
		/// Determina si esta celda evita que un objeto pueda entrar.
		/// </summary>
		public bool Collision (ICellObject collObj)
		{
			return Objects.Any (z => z.Collision (collObj));
		}

		public Cell ()
		{
			Objects = new List<ICellObject> ();
		}

		public void AddObject (ICellObject obj)
		{
			Objects.Add (obj);
		}

		public void RemoveObject (ICellObject obj)
		{
			Objects.Remove (obj);
		}

		/// <summary>
		/// Valida y realiza moviemiento.
		/// </summary>
		/// <returns><c>true</c>, if object was moved to cell, <c>false</c> otherwise.</returns>
		/// <param name="obj">Objeto a mover</param>
		/// <param name="newCell">New cell.</param>
		public bool MoveObjectToCell (ICellObject obj, Cell newCell)
		{
			if (newCell.Collision (obj))
				return false;
			RemoveObject (obj);
			newCell.AddObject (obj);
			return true;
		}

		public void Dibujar (SpriteBatch bat, Rectangle rect)
		{
			//var x = Objects [r.Next (Objects.Count)];
			//var x = Objects [Objects.Count - 1];
			foreach (var x in Objects)
				bat.Draw (
					x.Texture,
					rect, null, x.UseColor ?? Color.White, 
					0, Vector2.Zero,
					SpriteEffects.None,
					x.Depth);
				
		}

		public void LoadContent ()
		{
			foreach (var x in Objects)
				x.LoadContent ();
		}
	}
}