using System;
using System.Diagnostics;
using Debugging;
using Items;
using Items.Declarations;
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


			#region Tmp
			var knife = ItemFactory.CreateItem (ItemType.Knife);
			MyGame.Items = new ItemDatabase (new []{ knife });
			var mapJson = JsonConvert.SerializeObject (MyGame.Items, CommonItemBase.JsonSettings);

			var j = JsonConvert.DeserializeObject<ItemDatabase> (mapJson, CommonItemBase.JsonSettings);
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