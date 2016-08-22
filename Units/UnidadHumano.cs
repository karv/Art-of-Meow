using Cells.CellObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Cells;

namespace Units
{
	public class UnidadHumano : IUnidad
	{
		public const string TextureType = "person";
		readonly ContentManager _content;

		public string TextureStr { get; protected set; }

		public Texture2D Texture { get; protected set; }


		public UnidadHumano (ContentManager content, string texture = TextureType)
		{
			_content = content;
			TextureStr = texture;
		}

		public void LoadContent ()
		{
			_content.Load<Texture2D> (TextureStr);
		}

		public bool Collision (ICellObject collObj)
		{
			return false; // TODO
		}

		public void Draw (Rectangle area, SpriteBatch bat)
		{
			bat.Draw (
				Texture,
				area, null, Color.White,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Unidad);
		}

		public void Dispose ()
		{
			Texture = null;
		}

		public Point Location { get; set; }
	}
}