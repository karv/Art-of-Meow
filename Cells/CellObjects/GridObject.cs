using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Units;

namespace Cells.CellObjects
{
	public class GridObject : IGridObject
	{
		public readonly string StringTexture;

		public Color UseColor { get; set; }

		public float Depth { get; set; }

		public Texture2D Texture { get; private set; }

		public GridObject (string texture, Grid grid)
		{
			StringTexture = texture;
			CollidePlayer = false;
			Grid = grid;
		}

		public void UnloadContent ()
		{
		}

		public Grid Grid { get; }

		public Moggle.Controles.IComponentContainerComponent<Moggle.Controles.IControl> Container
		{ get { return Grid as Moggle.Controles.IComponentContainerComponent<Moggle.Controles.IControl>; } }

		public virtual void Initialize ()
		{
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

		public void Draw (SpriteBatch bat, Rectangle area)
		{
			if (StringTexture == "brick-wall")
				System.Console.WriteLine ();
			bat.Draw (
				Texture,
				area, null, UseColor,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depth);
		}

		public void Dispose ()
		{
			Texture = null;
		}

		public Point Location { get; set; }

		public override string ToString ()
		{
			return string.Format (
				"[GridObject: StringTexture={0}, UseColor={1}, CollidePlayer={2}, Location={3}]",
				StringTexture,
				UseColor,
				CollidePlayer,
				Location);
		}
		
	}
}