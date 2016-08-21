using Moggle.Screens;
using Microsoft.Xna.Framework;

namespace Art_of_Meow
{
	public class Juego : Moggle.Game
	{
		public Juego ()
		{
			Graphics.IsFullScreen = false;
		}
	}

	public class SomeScreen : Screen
	{
		public override Color BgColor
		{
			get
			{
				return Color.Blue;
			}
		}

		public SomeScreen (Moggle.Game game)
			: base (game)
		{
		}
		
	}

	public class Start
	{
		public static Juego MyGame = new Juego ();

		public static void Main ()
		{
			MyGame.CurrentScreen = new SomeScreen (MyGame);
			MyGame.CurrentScreen.Inicializar ();
			MyGame.Run ();
		}
	}
}