using System.Linq;
using AoM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moggle.Comm;
using Moggle.Controles;
using MonoGame.Extended.InputListeners;
using Screens;
using Units;

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

			if (GlobalKeys.GameExit.Contains (key))
			{
				Juego.Exit ();
				return true;
			}
			if (GlobalKeys.OpenWindowInventory.Contains (key))
			{
				if (HumanPlayer.Inventory.Any () || HumanPlayer.Equipment.EnumerateEquipment ().Any ())
				{
					var scr = new EquipmentScreen (ManagerScreen, HumanPlayer);
					scr.Execute (ScreenExt.DialogOpt);
				}
				return true;
			}
			if (GlobalKeys.Center.Contains (key))
			{
				ManagerScreen.GridControl.TryCenterOn (HumanPlayer.Location);
				return true;
			}
			if (GlobalKeys.OpenWindowSkills.Contains (key))
			{
				if (HumanPlayer.Skills.AnyUsable ())
				{
					var scr = new InvokeSkillListScreen (Juego, HumanPlayer);
					scr.Execute (ScreenExt.DialogOpt);
				}
				return true;
			}
			if (GlobalKeys.Stairs.Contains (key))
			{
				var endPoint = ManagerScreen.Grid.LocalTopology.EndPointOf (
					               HumanPlayer.Location,
					               HumanPlayer.Exp.ExperienciaTotal * 0.8f);
				if (endPoint.HasValue)
					ManagerScreen.ChangeGrid (endPoint.Value);
			}
			return false;
		}

		#endregion

		#region IComponent implementation

		/// <summary>
		/// Loads the content using a given manager
		/// </summary>
		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
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