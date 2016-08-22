
namespace Art_of_Meow
{
	public class Program
	{
		public static Juego MyGame = new Juego ();

		public static void Main ()
		{
			MyGame.CurrentScreen = new Screens.MapMainScreen (MyGame);
			MyGame.CurrentScreen.Inicializar ();
			MyGame.CurrentScreen.Escuchando = true;
			MyGame.Run ();
		}
	}
}