using System;
using System.Diagnostics;
using Debugging;
using Items;

namespace AoM
{
	/// <summary>
	/// Main program
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The game engine
		/// </summary>
		public static Juego MyGame = new Juego ();

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		public static void Main ()
		{
			ItemFactory.ContentManager = MyGame.Contenido;
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