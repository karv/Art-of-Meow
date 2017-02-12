using Microsoft.Xna.Framework;
using System;

namespace Units
{

	/// <summary>
	/// Extends the <see cref="MovementDirectionEnum"/> enum
	/// </summary>
	public static class Movement
	{
		/// <summary>
		/// Gets the destination point when moving as a <see cref="MovementDirectionEnum"/>
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">The movement destination</param>
		public static MovementDirectionEnum GetDirectionTo (this Point @from,
		                                                    Point to)
		{
			var pdir = to - from;
			return pdir.GetDirection ();
		}

		static MovementDirectionEnum GetDirection (this Point point)
		{
			if (point.X > 0)
				return point.Y > 0 ?
					MovementDirectionEnum.DownRight :
					point.Y == 0 ? 
					MovementDirectionEnum.Right :
					MovementDirectionEnum.UpRight;

			if (point.X == 0)
				return point.Y > 0 ?
					MovementDirectionEnum.Down :
					point.Y == 0 ? 
					MovementDirectionEnum.NoMov :
					MovementDirectionEnum.Up;

			return point.Y > 0 ?
					MovementDirectionEnum.DownLeft :
					point.Y == 0 ? 
					MovementDirectionEnum.Left :
					MovementDirectionEnum.UpLeft;
		}

		/// <summary>
		/// Rota la dirección rotation*45° hacia la izquierda.
		/// </summary>
		public static MovementDirectionEnum Rotate (this MovementDirectionEnum dir,
		                                            int rotation)
		{
			var int_dir = ((int)dir + rotation) % (int)MovementDirectionEnum.__total_dirs;
			return (MovementDirectionEnum)int_dir;
		}

		/// <summary>
		/// Converts a <see cref="MovementDirectionEnum"/> into a <see cref="Point"/>
		/// </summary>
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