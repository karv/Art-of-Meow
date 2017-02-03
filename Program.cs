using System;
using System.Diagnostics;
using Debugging;
using Items.Modifiers;
using Maps;
using MonoGame.Extended;
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
			var lg = new Logger ("debug.log");
			Debug.Listeners.Add (lg);


			#region Tmp
			var map = new Map (new Size (3, 3));
			map.MapItemGroundItems = new Helper.ProbabilityInstanceSet<IItemFactory> ();
			map.MapItemGroundItems.Add (new RandomItemRecipe
			{
				MinItemVal = 0,
				MaxItemVal = 100,
				AllowedTypes = new [] { ItemType.HealingPotion }
			}, 0.6f);

			var mapJson = Newtonsoft.Json.JsonConvert.SerializeObject (map, Map.JsonSets);
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