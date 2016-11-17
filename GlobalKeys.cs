using Microsoft.Xna.Framework.Input;

namespace AoM
{
	/// <summary>
	/// Preferencai de botones
	/// </summary>
	public static class GlobalKeys
	{
		#region Movimiento

		public static Keys [] MovUpKey = { Keys.Up, Keys.NumPad8 };
		public static Keys [] MovDownKey = { Keys.Down, Keys.NumPad2 };
		public static Keys [] MovLeftKey = { Keys.Left, Keys.NumPad4 };
		public static Keys [] MovRightKey = { Keys.Right, Keys.NumPad6 };
		public static Keys [] MovUpRightKey = { Keys.NumPad9 };
		public static Keys [] MovUpLeftKey = { Keys.NumPad7 };
		public static Keys [] MovDownRightKey = { Keys.NumPad3 };
		public static Keys [] MovDownLeftKey = { Keys.NumPad1 };
		public static Keys [] Stairs = { Keys.Enter };

		#endregion

		#region Special

		public static Keys [] GameExit = { Keys.Escape };
		public static Keys [] Center = { Keys.C };

		#endregion

		#region Debug

		#if DEBUG
		public static Keys [] PrintRec = { Keys.Tab };
		#endif
		#endregion

		#region Map actions

		public static Keys [] PickupDroppedItems = { Keys.OemPeriod };

		#endregion

		#region Windows

		public static Keys [] OpenWindowInventory = { Keys.I };
		public static Keys [] OpenWindowSkills = { Keys.S };

		public static Keys [] SelectUpKey = { Keys.Up };
		public static Keys [] SelectMovDownKey = { Keys.Down };
		public static Keys [] SelectMovLeftKey = { Keys.Left };
		public static Keys [] SelectMovRightKey = { Keys.Right };
		public static Keys [] SelectMovUpRightKey = { };
		public static Keys [] SelectMovUpLeftKey = { };
		public static Keys [] SelectMovDownRightKey = { };
		public static Keys [] SelectMovDownLeftKey = { };

		#endregion
	}
}