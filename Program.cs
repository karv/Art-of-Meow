using System;
using System.Diagnostics;
using Debugging;
using Items.Modifiers;
using Maps;

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

			var br = new ItemModifier ("Broken", ItemModifierNameUsage.Prefix, 
				         new []
				{
					new ItemModification (Items.ConstantesAtributos.Ataque, -1.2f),
					new ItemModification (Items.ConstantesAtributos.Hit, -0.1f)
				});
			var sr = Newtonsoft.Json.JsonConvert.SerializeObject (br, Map.JsonSets);
			var br2 = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemModifier> (sr);
			Debug.Listeners.Add (lg);
			var MapThread = MyGame.ScreenManager.AddNewThread ();
			MapThread.Stack (new Screens.MapMainScreen (MyGame));
			MyGame.Run ();
			lg.WriteInAll ("=== End of log ===");
			lg.Close ();
			Environment.Exit (0);
		}
	}
}