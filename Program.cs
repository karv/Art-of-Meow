using System;
using System.Diagnostics;
using Debugging;

namespace AoM
{
	public class Program
	{
		public static Juego MyGame = new Juego ();

		public static void Main ()
		{
			var lg = new Logger ("debug.log");
			Debug.Listeners.Add (lg);
			MyGame.CurrentScreen = new Screens.MapMainScreen (MyGame);
			MyGame.Run ();
			lg.WriteInAll ("=== End of log ===");
			lg.Close ();
			Environment.Exit (0);
		}
	}
}