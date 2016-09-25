
namespace Units.Inteligencia
{
	/// <summary>
	/// Permite al jugador interactuar con su unidad.
	/// </summary>
	public class HumanIntelligence : IIntelligence, Moggle.Comm.IReceptorTeclado
	{
		public readonly Unidad Yo;

		void IIntelligence.DoAction ()
		{
			if (ActionDir != MovementDirectionEnum.NoMov)
			{
				Yo.MoveOrMelee (ActionDir);
				ActionDir = MovementDirectionEnum.NoMov;
			}
		}

		MovementDirectionEnum ActionDir;

		bool Moggle.Comm.IReceptorTeclado.RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs keyArg)
		{
			var key = keyArg.Key;
			switch (key)
			{
				case Microsoft.Xna.Framework.Input.Keys.Down:
				case Microsoft.Xna.Framework.Input.Keys.NumPad2:
				case Microsoft.Xna.Framework.Input.Keys.K:
					ActionDir = MovementDirectionEnum.Down;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.Right:
				case Microsoft.Xna.Framework.Input.Keys.NumPad6:
				case Microsoft.Xna.Framework.Input.Keys.O:
					ActionDir = MovementDirectionEnum.Right;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.Up:
				case Microsoft.Xna.Framework.Input.Keys.NumPad8:
				case Microsoft.Xna.Framework.Input.Keys.D8:
					ActionDir = MovementDirectionEnum.Up;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.Left:
				case Microsoft.Xna.Framework.Input.Keys.NumPad4:
				case Microsoft.Xna.Framework.Input.Keys.U:
					ActionDir = MovementDirectionEnum.Left;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad1:
				case Microsoft.Xna.Framework.Input.Keys.J:
					ActionDir = MovementDirectionEnum.DownLeft;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad3:
				case Microsoft.Xna.Framework.Input.Keys.L:
					ActionDir = MovementDirectionEnum.DownRight;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad7:
				case Microsoft.Xna.Framework.Input.Keys.D7:
					ActionDir = MovementDirectionEnum.UpLeft;
					return true;
				case Microsoft.Xna.Framework.Input.Keys.NumPad9:
				case Microsoft.Xna.Framework.Input.Keys.D9:
					ActionDir = MovementDirectionEnum.UpRight;
					return true;
			}
			return false;
		}

		public override string ToString ()
		{
			return string.Format ("[HumanIntelligence: Yo={0}]", Yo.Nombre);
		}

		public HumanIntelligence (Unidad yo)
		{
			Yo = yo;
		}
	}
	
}
