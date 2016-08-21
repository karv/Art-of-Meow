
namespace Art_of_Meow
{
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