using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Art_of_Meow
{
	public class Juego : Moggle.Game
	{
		#region Texturas definidas

		public Texture2D SolidTexture { get; private set; }

		#endregion

		public Juego ()
		{
			buildTextures ();
			Graphics.IsFullScreen = true;
		}

		void buildTextures ()
		{
			SolidTexture = new Texture2D (GraphicsDevice, 1, 1);
			var data = new Color[1];
			data [0] = Color.White;
			SolidTexture.SetData (data);
		}
	}
}