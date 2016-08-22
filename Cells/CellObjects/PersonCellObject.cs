using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Cells.CellObjects
{
	[ObsoleteAttribute]
	public class PersonCellObject : ICellObject
	{
		public readonly string StringTexture;
		ContentManager _content;

		public Texture2D Texture { get; private set; }

		public PersonCellObject (string texture, ContentManager content)
		{
			StringTexture = texture;
			_content = content;
		}

		public PersonCellObject (Texture2D texture)
		{
			Texture = texture;
			StringTexture = Texture.Name;
		}

		bool ICellObject.Collision (ICellObject collObj)
		{
			return true;
		}

		public void LoadContent ()
		{
			Texture = _content.Load<Texture2D> (StringTexture);
		}

		public float Depth { get { return Depths.Unidad; } }

		public void Draw (Microsoft.Xna.Framework.Rectangle area, SpriteBatch bat)
		{
			throw new System.NotImplementedException ();
		}

		public void Dispose ()
		{
			throw new System.NotImplementedException ();
		}

		public Microsoft.Xna.Framework.Point Location { get; set; }
	}
}