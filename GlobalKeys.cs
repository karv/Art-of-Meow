using Microsoft.Xna.Framework.Input;

namespace AoM
{
	/// <summary>
	/// Preferencai de botones
	/// </summary>
	public class GlobalKeys
	{
		#region Movimiento

		public Keys [] MovUpKey = { Keys.Up, Keys.NumPad8 };
		public Keys [] MovDownKey = { Keys.Down, Keys.NumPad2 };
		public Keys [] MovLeftKey = { Keys.Left, Keys.NumPad4 };
		public Keys [] MovRightKey = { Keys.Right, Keys.NumPad6 };
		public Keys [] MovUpRightKey = { Keys.NumPad9 };
		public Keys [] MovUpLeftKey = { Keys.NumPad7 };
		public Keys [] MovDownRightKey = { Keys.NumPad3 };
		public Keys [] MovDownLeftKey = { Keys.NumPad1 };
		public Keys [] Stairs = { Keys.Enter };

		#endregion

		#region Map actions

		public Keys [] PickupDroppedItems = { Keys.OemPeriod };

		#endregion

		#region Windows

		public Keys [] OpenWindowInventory = { Keys.I };
		public Keys [] OpenWindowSkills = { Keys.S };

		public Keys [] SelectUpKey = { Keys.Up };
		public Keys [] SelectMovDownKey = { Keys.Down };
		public Keys [] SelectMovLeftKey = { Keys.Left };
		public Keys [] SelectMovRightKey = { Keys.Right };
		public Keys [] SelectMovUpRightKey = { };
		public Keys [] SelectMovUpLeftKey = { };
		public Keys [] SelectMovDownRightKey = { };
		public Keys [] SelectMovDownLeftKey = { };


		#endregion
	}
}