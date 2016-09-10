using System.Threading;
using Debugging;
using System.Diagnostics;


namespace AoM
{
	public class Program
	{
		public static Juego MyGame = new Juego ();

		public static void Main ()
		{
			var lg = new Logger ("debug.log");
			Debug.Listeners.Add (lg);
			Debug.WriteLine ("Iniciando");
			MyGame.CurrentScreen = new Screens.MapMainScreen (MyGame);
			MyGame.Run ();
		}
	}
}