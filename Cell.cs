using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Art_of_Meow
{

	public class Cell
	{
		readonly ICollection<ICellObject> Objects;

		public Cell ()
		{
			Objects = new List<ICellObject> ();
		}

		public void Dibujar (SpriteBatch bat, Rectangle rect)
		{
			foreach (var x in Objects)
				bat.Draw (
					x.Texture,
					rect, null, Color.White, 
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