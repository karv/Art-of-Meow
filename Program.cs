using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Debugging;
using Helper;
using Maps;
using Newtonsoft.Json;
using Units;
using Units.Recursos;

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

			MyGame.ClassRaceManager = UnitClassRaceManager.FromFile (FileNames.RaceClass);
			// read races and classes
			var warrStats = new Dictionary<string, float> ();
			warrStats.Add (ConstantesRecursos.CertezaMelee, 0.16f);
			warrStats.Add (ConstantesRecursos.Destreza, 0.16f);
			warrStats.Add (ConstantesRecursos.EvasiónMelee, 0.08f);
			warrStats.Add (ConstantesRecursos.EvasiónRango, 0.04f);
			warrStats.Add (ConstantesRecursos.Fuerza, 0.24f);
			warrStats.Add (ConstantesRecursos.HP, 0.16f);
			warrStats.Add (ConstantesRecursos.Velocidad, 0.08f);
			var warriorClass = new UnitClass ("Warrior", warrStats);

			warrStats = new Dictionary<string, float> ();
			warrStats.Add (ConstantesRecursos.CertezaMelee, 1f);
			warrStats.Add (ConstantesRecursos.Destreza, 0.7f);
			warrStats.Add (ConstantesRecursos.EvasiónMelee, 1f);
			warrStats.Add (ConstantesRecursos.EvasiónRango, 0.8f);
			warrStats.Add (ConstantesRecursos.Fuerza, 0.2f);
			warrStats.Add (ConstantesRecursos.HP, 0.4f);
			warrStats.Add (ConstantesRecursos.Velocidad, 0.4f);
			var alienRace = new UnitRace (
				                "Imp",
				                new [] { "Warrior" },
				                new ReadOnlyDictionary<string, float> (warrStats),
				                "swordman");
			MyGame.ClassRaceManager = new UnitClassRaceManager (
				new [] { warriorClass },
				new [] { alienRace });

			var rcJson = JsonConvert.SerializeObject (MyGame.ClassRaceManager, UnitClassRaceManager.JsonSets);
			Console.WriteLine (rcJson);
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
			//Debug.WriteLine (json);
			var mp = Map.ReadFromFile (Map.MapDir + @"/base.map.json");
			mp.Populator = pop;
			json = JsonConvert.SerializeObject (mp, Map.JsonSets);
			Debug.WriteLine (json);
			var clone = JsonConvert.DeserializeObject<Map> (json, Map.JsonSets);
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