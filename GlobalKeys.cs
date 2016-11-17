using Microsoft.Xna.Framework.Input;

namespace AoM
{
	/// <summary>
	/// Preferencai de botones
	/// </summary>
	public static class GlobalKeys
	{
		#region Movimiento

		/// <summary>
		/// Movimiento del jugador hacia arriba
		/// </summary>
		public static Keys [] MovUpKey = { Keys.Up, Keys.NumPad8 };
		/// <summary>
		/// Movimiento del jugador hacia abajo
		/// </summary>
		public static Keys [] MovDownKey = { Keys.Down, Keys.NumPad2 };
		/// <summary>
		/// Movimiento del jugador hacia izquierda
		/// </summary>
		public static Keys [] MovLeftKey = { Keys.Left, Keys.NumPad4 };
		/// <summary>
		/// Movimiento del jugador hacia derecha
		/// </summary>
		public static Keys [] MovRightKey = { Keys.Right, Keys.NumPad6 };
		/// <summary>
		/// Movimiento del jugador hacia arriba-derecha
		/// </summary>
		public static Keys [] MovUpRightKey = { Keys.NumPad9 };
		/// <summary>
		/// Movimiento del jugador hacia arriba-izquierda
		/// </summary>
		public static Keys [] MovUpLeftKey = { Keys.NumPad7 };
		/// <summary>
		/// Movimiento del jugador hacia abajo-derecha
		/// </summary>
		public static Keys [] MovDownRightKey = { Keys.NumPad3 };
		/// <summary>
		/// Movimiento del jugador hacia abajo-izquierda
		/// </summary>
		public static Keys [] MovDownLeftKey = { Keys.NumPad1 };
		/// <summary>
		/// Usar escaleras
		/// </summary>
		public static Keys [] Stairs = { Keys.Enter };

		#endregion

		#region Special

		/// <summary>
		/// Salir del juego
		/// </summary>
		public static Keys [] GameExit = { Keys.Escape };
		/// <summary>
		/// Centrar en el jugador
		/// </summary>
		public static Keys [] Center = { Keys.C };

		#endregion

		#region Debug

		#if DEBUG
		/// <summary>
		/// (Debug)Imprimir en la salida Debug los recursos del jugador
		/// </summary>
		public static Keys [] PrintRec = { Keys.Tab };
		#endif
		#endregion

		#region Map actions

		/// <summary>
		/// Recoger items
		/// </summary>
		public static Keys [] PickupDroppedItems = { Keys.OemPeriod };

		#endregion

		#region Windows

		/// <summary>
		/// Abrir la ventana de Inventory
		/// </summary>
		public static Keys [] OpenWindowInventory = { Keys.I };
		/// <summary>
		/// Abrir la ventana de skils
		/// </summary>
		public static Keys [] OpenWindowSkills = { Keys.S };

		/// <summary>
		/// Seleccionar arriba
		/// </summary>
		public static Keys [] SelectUpKey = { Keys.Up };
		/// <summary>
		/// Seleccionar abajo
		/// </summary>
		public static Keys [] SelectMovDownKey = { Keys.Down };
		/// <summary>
		/// Seleccionar izquierda
		/// </summary>
		public static Keys [] SelectMovLeftKey = { Keys.Left };
		/// <summary>
		/// Seleccionar derecha
		/// </summary>
		public static Keys [] SelectMovRightKey = { Keys.Right };

		#endregion
	}
}