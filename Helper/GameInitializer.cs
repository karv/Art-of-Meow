﻿using AoM;
using Cells;
using Items.Declarations.Equipment;
using Maps;
using Microsoft.Xna.Framework;
using Units;
using Units.Inteligencia;

namespace Helper
{
	/// <summary>
	/// Se encarga de inicializar el juego
	/// </summary>
	public static class GameInitializer
	{
		/// <summary>
		/// Gets the name of the first map
		/// </summary>
		public const string FirstMap = FileNames.MapFolder + @"/base.map.json";

		static Unidad buildPlayer (LogicGrid grid)
		{
			const int startingExp = 3;
			var player = new Unidad (grid)
			{
				Nombre = "Player",
				Team = new TeamManager (Color.Red),
				Location = grid.GetRandomEmptyCell ()
			};
			player.Inteligencia = new HumanIntelligence (player);

			var uClass = Program.MyGame.ClassRaceManager.Class [0];
			var uRace = Program.MyGame.ClassRaceManager.Race [0];
			foreach (var x in UnitRace.mergeAttrDists (uRace, uClass))
				player.Exp.AddAssignation (x.Key, x.Value);
			player.Exp.ExperienciaAcumulada = startingExp;
			player.Exp.Flush ();

			#region Cheat
			var eq = Program.MyGame.Items.CreateItem<MeleeWeapon> ("Knife");
			player.Inventory.Add (eq);
			eq.Modifiers.Modifiers.Add (Program.MyGame.ItemMods ["Broken"]);
			player.Equipment.EquipItem (eq);

			player.Skills.AddSkill (Program.MyGame.SkillList.GetSkill ("Rage"));

			#endregion

			player.Initialize ();
			return player;
		}

		/// <summary>
		/// Inicializa un nuevo mundo
		/// </summary>
		/// <param name="player">El jugador humano generado</param>
		public static LogicGrid InitializeNewWorld (out Unidad player)
		{
			Map map;
			//var ret = Map.GenerateGrid (FirstMap, 0);
			//map = Map.HardCreateNew ();
			//var json = map.ToJSON ();
			//Debug.WriteLine (json);
			//map = Map.ReadFromJSON (json);
			map = Map.ReadFromFile (FirstMap);

			/*
			#region tmp
			var code = File.ReadAllText (@"Data/Maps/Script para CSharp.js");
			var eng = new Jurassic.ScriptEngine ();
			var t = eng.Evaluate (code);
			var r = eng.CallGlobalFunction ("obtenerJSON") as string;
			map = Map.ReadFromJSON (r);
			#endregion
*/

			var ret = map.GenerateGrid (0);

			player = buildPlayer (ret);
			ret.AddCellObject (player);
			return ret;
		}
	}
}