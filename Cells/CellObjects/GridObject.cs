using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Units;
using MonoGame.Extended.Shapes;

namespace Cells.CellObjects
{
	public class GridObject : IGridObject
	{
		public readonly string StringTexture;

		public Color UseColor { get; set; }

		public float Depth { get; set; }

		public Texture2D Texture { get; private set; }

		public GridObject (string texture)
		{
			StringTexture = texture;
			CollidePlayer = false;
		}

		public bool CollidePlayer { get; set; }

		public bool Collision (IGridObject collObj)
		{
			return CollidePlayer && collObj is IUnidad;
		}

		public void LoadContent (ContentManager content)
		{
			Texture = content.Load<Texture2D> (StringTexture);
		}

		public void Draw (RectangleF area, SpriteBatch bat)
		{
			// TODO:
			var ar = area.ToRectangle ();
			bat.Draw (
				Texture,
				ar, null, UseColor,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depth);
		}

		public void Dispose ()
		{
			Texture = null;
		}

		public Point Location { get; set; }
	}
}