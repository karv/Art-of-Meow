using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cells.CellObjects
{
	public class BackgroundObject : IGridObject
	{
		public readonly string StringTexture;

		public Texture2D Texture { get; private set; }

		public void UnloadContent ()
		{
		}

		public Grid Grid { get; }

		public Moggle.Controles.IComponentContainerComponent<Moggle.Controles.IControl> Container { get; }

		bool IGridObject.Collision (IGridObject collObj)
		{
			return false;
		}

		public void LoadContent (ContentManager content)
		{
			Texture = content.Load<Texture2D> (StringTexture);
		}

		public void Initialize ()
		{
		}

		public Color? UseColor { get { return Color.White; } }

		public float Depth { get { return Depths.Background; } }

		public void Draw (SpriteBatch bat, Rectangle area)
		{
			bat.Draw (
				Texture,
				area, null, Color.White,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Background);
		}

		public Point Location { get; set; }

		public void Dispose ()
		{
			Texture = null;
		}

		public override string ToString ()
		{
			return string.Format (
				"[BackgroundObject: StringTexture={0}, Color={1}, Location={2}]",
				StringTexture,
				UseColor,
				Location);
		}


		public BackgroundObject (Point loc,
		                         string texture, 
		                         Grid grid)
		{
			StringTexture = texture;
			Location = loc;
			Grid = grid;
		}
	}
}