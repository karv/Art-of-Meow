
namespace Art_of_Meow
{
	public class Program
	{
		public static Juego MyGame = new Juego ();

		public static void Main ()
		{
			MyGame.CurrentScreen = new Screens.SomeScreen (MyGame);
			MyGame.CurrentScreen.Inicializar ();
			MyGame.Run ();
		}
	}
}