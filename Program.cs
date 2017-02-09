using System;
using System.Diagnostics;
using Debugging;
using Maps;
using Helper;
using Newtonsoft.Json;

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

			#region Test


			var rule = new PopulationRule
			{
				Chance = 0.9f,
				Stacks = new []
				{
					new DistributedStack
					{
						RaceName = "Alien",
						UnitQuantityDistribution = new IntegerInterval (3, 5)
					}
				}
			};
			var pop = new Populator (new [] { rule });

			var json = JsonConvert.SerializeObject (pop, Map.JsonSets);
			Debug.WriteLine (json);
			var clone = JsonConvert.DeserializeObject<Populator> (json, Map.JsonSets);
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