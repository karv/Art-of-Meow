using Cells;
using Items;
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
		public const string FirstMap = @"Maps/base.map";

		static Unidad buildPlayer (LogicGrid grid)
		{
			var player = new Unidad (grid)
			{
				Nombre = "Player",
				Team = new TeamManager (Color.Red),
				Location = grid.GetRandomEmptyCell ()
			};

			player.Inteligencia = new HumanIntelligence (player);

			#region Cheat
			var eq = ItemFactory.CreateItem<IEquipment> (ItemType.Knife);
			//eq.Modifiers.Modifiers.Add (ItemModifierGenerator.Broken);
			player.Equipment.EquipItem (eq);
			player.Inventory.Add (ItemFactory.CreateItem (ItemType.Bow));
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
			var ret = Map.GenerateGrid (FirstMap, 0);
			player = buildPlayer (ret);
			ret.AddCellObject (player);
			return ret;
		}
	}
}