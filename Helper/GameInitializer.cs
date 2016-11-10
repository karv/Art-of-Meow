using Cells;
using Items;
using Maps;
using Microsoft.Xna.Framework;
using Screens;
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

		static Unidad buildPlayer (Grid grid)
		{
			var player = new Unidad (grid)
			{
				Nombre = "Player",
				Team = new TeamManager (Color.Red),
				Location = new Point (
					grid.GridSize.Width / 2,
					grid.GridSize.Height / 2),
			};

			player.Inteligencia = new HumanIntelligence (player);
			player.Equipment.EquipItem (ItemFactory.CreateItem (ItemType.Sword) as IEquipment);

			return player;
		}

		/// <summary>
		/// Inicializa un nuevo mundo
		/// </summary>
		/// <param name="scr">Pantalla</param>
		/// <param name="player">Player.</param>
		public static Grid InitializeNewWorld (MapMainScreen scr, out Unidad player)
		{
			var ret = Map.GenerateGrid (FirstMap, scr);
			player = buildPlayer (ret);
			return ret;
		}
	}
}