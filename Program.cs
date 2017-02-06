using System;
using System.Diagnostics;
using Debugging;
using Items;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Items.Declarations.Equipment;

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
			var lg = new Logger ("debug.log");
			Debug.Listeners.Add (lg);


			#region Tmp
			var mp = new RandomItemRecipe ();
			var json = JsonConvert.SerializeObject (mp, ItemDatabase.JsonSettings);
			Debug.WriteLine (json);
			var mp2 = JsonConvert.DeserializeObject<RandomItemRecipe> (json, ItemDatabase.JsonSettings);
			if (mp2.AllowedItemNames == null)
				throw new Exception ();
			#endregion

			var MapThread = MyGame.ScreenManager.AddNewThread ();
			MapThread.Stack (new Screens.MapMainScreen (MyGame));
			MyGame.Run ();

			lg.WriteInAll ("=== End of log ===");
			lg.Close ();
			Environment.Exit (0);
		}
	}
}