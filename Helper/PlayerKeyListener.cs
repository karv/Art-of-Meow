using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Moggle.Comm;
using Moggle.Controles;
using MonoGame.Extended.InputListeners;
using Screens;
using Units;
using Microsoft.Xna.Framework;

namespace Helper
{
	/// <summary>
	/// Ayuda con las teclas rápidas especiales en <see cref="Screens.MapMainScreen"/>
	/// </summary>
	public class PlayerKeyListener : IReceptor<KeyboardEventArgs>, IComponent
	{
		Unidad HumanPlayer { get { return ManagerScreen.Player; } }

		MapMainScreen ManagerScreen { get; }

		Moggle.Game Juego { get { return ManagerScreen.Juego; } }


		#region IReceptor implementation

		/// <summary>
		/// Rebice señal de un tipo dado
		/// </summary>
		/// <param name="keyArg">Señal de tecla</param>
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
						scr.Execute (ScreenExt.DialogOpt);
					}
					break;
				case Keys.C:
					ManagerScreen.GridControl.TryCenterOn (HumanPlayer.Location);
					break;
				case Keys.S:
					if (HumanPlayer.Skills.AnyUsable ())
					{
						var scr = new InvokeSkillListScreen (Juego, HumanPlayer);
						scr.Execute (ScreenExt.DialogOpt);
					}
					break;
			}
			return false;
		}

		#endregion

		
		#region IComponent implementation

		void IComponent.AddContent ()
		{
		}

		void IComponent.InitializeContent ()
		{
		}

		#endregion

		#region IGameComponent implementation

		void IGameComponent.Initialize ()
		{
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Helper.PlayerKeyListener"/> class.
		/// </summary>
		public PlayerKeyListener (MapMainScreen scr)
		{
			ManagerScreen = scr;
		}
	}
}

