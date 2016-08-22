using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Cells.CellObjects
{
	public class CellObject : ICellObject
	{
		public readonly string StringTexture;
		protected readonly ContentManager Content;

		public Color UseColor { get; set; }

		public float Depth { get; set; }

		public Texture2D Texture { get; private set; }

		public CellObject (string texture, ContentManager content)
		{
			StringTexture = texture;
			Content = content;
			CollidePlayer = false;
		}

		public bool CollidePlayer { get; set; }

		public bool Collision (ICellObject collObj)
		{
			return false; // TODO
		}

		public void LoadContent ()
		{
			Texture = Content.Load<Texture2D> (StringTexture);
		}

		public void Draw (Rectangle area, SpriteBatch bat)
		{
			bat.Draw (
				Texture,
				area, null, Color.White,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Background);
		}

		public void Dispose ()
		{
			Texture = null;
		}

		public Point Location { get; set; }
	}
}