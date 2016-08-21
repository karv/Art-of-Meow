using Microsoft.Xna.Framework.Graphics;

namespace Art_of_Meow
{

	public interface ICellObject
	{
		Texture2D Texture { get; }

		int Depth { get; }

		void LoadContent ();
	}
}