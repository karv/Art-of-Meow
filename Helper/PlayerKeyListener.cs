using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Moggle;
using Moggle.Comm;
using Moggle.Controles;
using MonoGame.Extended.InputListeners;
using Screens;
using Units;

namespace Helper
{
	public class PlayerKeyListener : IReceptor<KeyboardEventArgs>, IComponent
	{
		Unidad HumanPlayer { get { return ManagerScreen.Player; } }

		public MapMainScreen ManagerScreen { get; }

		Game Juego { get { return ManagerScreen.Juego; } }


		#region IReceptor implementation

		public bool RecibirSeñal (KeyboardEventArgs keyArg)
		{
			var key = keyArg.Key;

			switch (key)
			{
				case Keys.Escape:
					Juego.Exit ();
					break;
				case Keys.Tab:
					Debug.WriteLine (HumanPlayer.Recursos);
					break;
				case Keys.I:
					if (HumanPlayer.Inventory.Any () || HumanPlayer.Equipment.EnumerateEquipment ().Any ())
					{
						var scr = new EquipmentScreen (ManagerScreen, HumanPlayer);
						scr.Ejecutar ();
					}
					break;
				case Keys.C:
					ManagerScreen.GridControl.TryCenterOn (HumanPlayer.Location);
					break;
				case Keys.S:
					if (HumanPlayer.Skills.AnyUsable ())
					{
						var scr = new InvokeSkillListScreen (ManagerScreen, HumanPlayer);
						scr.Ejecutar ();
					}
					break;
			}
			return false;
		}

		#endregion

		
		#region IComponent implementation

		public void AddContent ()
		{
		}

		public void InitializeContent ()
		{
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
		}

		#endregion

		
		public PlayerKeyListener (MapMainScreen scr)
		{
			ManagerScreen = scr;
		}
	}
}

