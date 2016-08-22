using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Cells.CellObjects;
using System;

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

		public void MoveObjectToCell (ICellObject obj, Cell newCell)
		{
			RemoveObject (obj);
			newCell.AddObject (obj);
		}

		public void Dibujar (SpriteBatch bat, Rectangle rect)
		{
			//var x = Objects [r.Next (Objects.Count)];
			//var x = Objects [Objects.Count - 1];
			foreach (var x in Objects)
				//bat.Draw (x.Texture, rect, x.UseColor ?? Color.White);
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