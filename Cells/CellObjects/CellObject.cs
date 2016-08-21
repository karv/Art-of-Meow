using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Cells.CellObjects
{
	public class CellObject : ICellObject
	{
		public readonly string StringTexture;
		protected readonly ContentManager Content;

		public Color? UseColor { get; set; }

		public int Depth { get; set; }

		public Texture2D Texture { get; private set; }

		public CellObject (string texture, ContentManager content)
		{
			StringTexture = texture;
			Content = content;
		}

		public void LoadContent ()
		{
			Texture = Content.Load<Texture2D> (StringTexture);
		}
	}
}