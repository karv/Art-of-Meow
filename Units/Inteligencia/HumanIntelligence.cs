using System.Collections.Generic;
using System.Linq;
using AoM;
using Items;
using MonoGame.Extended.InputListeners;
using Units.Order;
using Microsoft.Xna.Framework;
using System;

namespace Units.Inteligencia
{
	/// <summary>
	/// Permite al jugador interactuar con su unidad.
	/// </summary>
	public class HumanIntelligence :
	IUnidadController,
	IGameComponent,
	IDisposable
	{
		/// <summary>
		/// The controlled unidad
		/// </summary>
		public readonly Unidad ControlledUnidad;

		void IUnidadController.DoAction ()
		{
			ControlledUnidad.assertIsIdleCheck ();
			if (ActionDir != MovementDirectionEnum.NoMov)
			{
				ControlledUnidad.MoveOrMelee (ActionDir);
				ActionDir = MovementDirectionEnum.NoMov;
			}
		}

		void IDisposable.Dispose ()
		{
			Program.MyGame.KeyListener.KeyTyped -= keyPressedListener;
		}

		MovementDirectionEnum ActionDir;

		void IGameComponent.Initialize ()
		{
			// Suscribirse
			Program.MyGame.KeyListener.KeyTyped += keyPressedListener;
		}

		void keyPressedListener (object sender, KeyboardEventArgs e)
		{
			recibirSeñal (e);
		}

		bool recibirSeñal (KeyboardEventArgs keyArg)
		{
			var key = keyArg.Key;
			if (GlobalKeys.MovUpKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.Up;
			}
			else if (GlobalKeys.MovDownKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.Down;
			}
			else if (GlobalKeys.MovLeftKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.Left;
			}
			else if (GlobalKeys.MovRightKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.Right;
			}
			else if (GlobalKeys.MovUpLeftKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.UpLeft;
			}
			else if (GlobalKeys.MovUpRightKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.UpRight;
			}
			else if (GlobalKeys.MovDownLeftKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.DownLeft;
			}
			else if (GlobalKeys.MovDownRightKey.Contains (key))
			{
				ActionDir = MovementDirectionEnum.DownRight;
			}
			else if (GlobalKeys.PickupDroppedItems.Contains (key))
			{
				// Tomar los objetos
				var objs = new List<GroundItem> (ControlledUnidad.Grid.GetCell (ControlledUnidad.Location).EnumerateObjects ().OfType<GroundItem> ());

				foreach (var x in objs)
				{
					ControlledUnidad.Inventory.Add (x.ItemClass);
					x.RemoveFromGrid ();
				}

				const float baseWaitTime = 0.2f;
				const float extraWaitTime = 0.07f;
				var waitTime = baseWaitTime + objs.Count * extraWaitTime;
				ControlledUnidad.EnqueueOrder (new CooldownOrder (
					ControlledUnidad,
					waitTime));
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Inteligencia.HumanIntelligence"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Inteligencia.HumanIntelligence"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[HI|{0}]",
				ControlledUnidad.Nombre);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Inteligencia.HumanIntelligence"/> class.
		/// </summary>
		/// <param name="yo">Controlled unidad</param>
		public HumanIntelligence (Unidad yo)
		{
			ControlledUnidad = yo;
		}
	}
}