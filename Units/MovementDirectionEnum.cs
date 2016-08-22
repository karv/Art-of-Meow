using Microsoft.Xna.Framework;
using System;


namespace Units
{
	public enum MovementDirectionEnum
	{
		NoMove,
		Up,
		Down,
		Left,
		Right,
		UpLeft,
		UpRight,
		DownLeft,
		DownRight
	}

	public static class Movement
	{
		public static Point AsDirectionalPoint (this MovementDirectionEnum dir)
		{
			switch (dir)
			{
				case MovementDirectionEnum.Down:
					return new Point (0, -1);
				case MovementDirectionEnum.DownLeft:
					return new Point (-1, -1);
				case MovementDirectionEnum.DownRight:
					return new Point (1, -1);
				case MovementDirectionEnum.Left:
					return new Point (-1, 0);
				case MovementDirectionEnum.NoMove:
					return new Point (0, 0);
				case MovementDirectionEnum.Right:
					return new Point (1, 0);
				case MovementDirectionEnum.Up:
					return new Point (0, 1);
				case MovementDirectionEnum.UpLeft:
					return new Point (-1, 1);
				case MovementDirectionEnum.UpRight:
					return new Point (1, 1);
				default:
					throw new Exception ();
			}
		}
	}
}