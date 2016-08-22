using Microsoft.Xna.Framework;
using System;


namespace Units
{
	public enum MovementDirectionEnum
	{
		Right,
		UpRight,
		Up,
		UpLeft,
		Left,
		DownLeft,
		Down,
		DownRight,
		__total_dirs
	}

	public static class Movement
	{
		/// <summary>
		/// Rota la dirección rotation*45° hacia la izquierda.
		/// </summary>
		public static MovementDirectionEnum Rotate (this MovementDirectionEnum dir,
		                                            int rotation)
		{
			var int_dir = ((int)dir + rotation) % (int)MovementDirectionEnum.__total_dirs;
			return (MovementDirectionEnum)int_dir;
		}



		public static Point AsDirectionalPoint (this MovementDirectionEnum dir)
		{
			switch (dir)
			{
				case MovementDirectionEnum.Down:
					return new Point (0, 1);
				case MovementDirectionEnum.DownLeft:
					return new Point (-1, 1);
				case MovementDirectionEnum.DownRight:
					return new Point (1, 1);
				case MovementDirectionEnum.Left:
					return new Point (-1, 0);
				case MovementDirectionEnum.Right:
					return new Point (1, 0);
				case MovementDirectionEnum.Up:
					return new Point (0, -1);
				case MovementDirectionEnum.UpLeft:
					return new Point (-1, -1);
				case MovementDirectionEnum.UpRight:
					return new Point (1, -1);
				default:
					throw new Exception ();
			}
		}
	}
}