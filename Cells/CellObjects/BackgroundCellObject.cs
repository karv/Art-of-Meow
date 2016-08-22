using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Cells.CellObjects
{
	public class BackgroundCellObject : ICellObject
	{
		public readonly string StringTexture;
		ContentManager _content;

		public Texture2D Texture { get; private set; }

		public BackgroundCellObject (string texture, ContentManager content)
		{
			StringTexture = texture;
			_content = content;
		}

		bool ICellObject.Collision (ICellObject collObj)
		{
			return false;
		}

		public void LoadContent ()
		{
			Texture = _content.Load<Texture2D> (StringTexture);
		}

		public Color? UseColor { get { return Color.White; } }

		public float Depth { get { return Depths.Background; } }
	}
}