using Microsoft.Xna.Framework;

namespace Units
{
	/// <summary>
	/// Permite al jugador interactuar con su unidad.
	/// </summary>
	public class HumanIntelligence : IIntelligence, Moggle.Comm.IReceptorTeclado, IGameComponent
	{
		public readonly Unidad Yo;

		public void DoAction ()
		{
			if (ActionDir != MovementDirectionEnum.NoMov)
			{
				Yo.MoveOrMelee (ActionDir);
				ActionDir = MovementDirectionEnum.NoMov;
			}
		}

		public void Initialize ()
		{
		}

		MovementDirectionEnum ActionDir;

		public bool RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs keyArg)
		{
			var key = keyArg.Key;
			switch (key)
			{
				case Microsoft.Xna.Framework.Input.Keys.Down:
				case Microsoft.Xna.Framework.Input.Keys.NumPad2:
					ActionDir = MovementDirectionEnum.Down;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.Right:
				case Microsoft.Xna.Framework.Input.Keys.NumPad6:
					ActionDir = MovementDirectionEnum.Right;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.Up:
				case Microsoft.Xna.Framework.Input.Keys.NumPad8:
					ActionDir = MovementDirectionEnum.Up;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.Left:
				case Microsoft.Xna.Framework.Input.Keys.NumPad4:
					ActionDir = MovementDirectionEnum.Left;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad1:
					ActionDir = MovementDirectionEnum.DownLeft;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad3:
					ActionDir = MovementDirectionEnum.DownRight;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad7:
					ActionDir = MovementDirectionEnum.UpLeft;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad9:
					ActionDir = MovementDirectionEnum.UpRight;
					return true;
			}
			return false;
		}

		public HumanIntelligence (Unidad yo)
		{
			Yo = yo;
		}
	}
	
}
