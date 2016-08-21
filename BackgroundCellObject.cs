using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Art_of_Meow
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

		public void LoadContent ()
		{
			Texture = _content.Load<Texture2D> ("floor");
		}

		public int Depth { get { return 0; } }
	}
}