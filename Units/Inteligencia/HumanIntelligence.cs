using System;
using System.Collections.Generic;
using System.Linq;
using AoM;
using Cells;
using Cells.CellObjects;
using Items;
using Microsoft.Xna.Framework;
using MonoGame.Extended.InputListeners;
using Units.Order;

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
		/// Devuelve o establece la memoria del jugador.
		/// </summary>
		/// <value>The memory.</value>
		public MemoryGrid Memory { get; set; }

		/// <summary>
		/// The controlled unidad
		/// </summary>
		public readonly Unidad ControlledUnidad;

		DateTime lastActionTime;
		/// <summary>
		/// Devuelve el tiempo de repetición
		/// </summary>
		public readonly TimeSpan MinRepetitionTime;

		bool shouldDoAction ()
		{
			if (forcedFirstHit || DateTime.Now - lastActionTime >= MinRepetitionTime)
			{
				lastActionTime = DateTime.Now;
				forcedFirstHit = false;
				return true;
			}
			return false;
		}

		void IUnidadController.DoAction ()
		{
			if (!shouldDoAction ())
				return;
			ControlledUnidad.assertIsIdleCheck ();
			if (PersistenceDir != MovementDirectionEnum.NoMov)
			{
				ControlledUnidad.MoveOrMelee (PersistenceDir);
				wasMoved ();
//				ActionDir = MovementDirectionEnum.NoMov;
			}
		}

		void wasMoved ()
		{
			Memory.UpdateMemory ();
		}

		void IDisposable.Dispose ()
		{
			Program.MyGame.KeyListener.KeyTyped -= keyPressedListener;
			Program.MyGame.KeyListener.KeyReleased -= freeAuntoKey;
		}

		MovementDirectionEnum PersistenceDir;
		bool forcedFirstHit;

		void IGameComponent.Initialize ()
		{
			// Suscribirse
			Program.MyGame.KeyListener.KeyPressed += keyPressedListener;
			Program.MyGame.KeyListener.KeyReleased += freeAuntoKey;
			PersistenceDir = MovementDirectionEnum.NoMov;
			lastActionTime = DateTime.Now;
		}

		void freeAuntoKey (object sender, KeyboardEventArgs e)
		{
			PersistenceDir = MovementDirectionEnum.NoMov;
		}

		bool shouldListen ()
		{
			return (ControlledUnidad as IUpdateGridObject).IsReady;
		}

		void keyPressedListener (object sender, KeyboardEventArgs e)
		{
			forcedFirstHit = true;
			if (shouldListen ())
				recibirSeñal (e);
		}

		bool recibirSeñal (KeyboardEventArgs keyArg)
		{
			var key = keyArg.Key;
			if (GlobalKeys.MovUpKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.Up;
			}
			else if (GlobalKeys.MovDownKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.Down;
			}
			else if (GlobalKeys.MovLeftKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.Left;
			}
			else if (GlobalKeys.MovRightKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.Right;
			}
			else if (GlobalKeys.MovUpLeftKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.UpLeft;
			}
			else if (GlobalKeys.MovUpRightKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.UpRight;
			}
			else if (GlobalKeys.MovDownLeftKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.DownLeft;
			}
			else if (GlobalKeys.MovDownRightKey.Contains (key))
			{
				PersistenceDir = MovementDirectionEnum.DownRight;
			}
			else if (GlobalKeys.WaitKey.Contains (key))
			{
				const float waitTime = 0.05f;
				ControlledUnidad.EnqueueOrder (new CooldownOrder (
					ControlledUnidad,
					waitTime));
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
			const int milisect_repeat = 150;
			MinRepetitionTime = TimeSpan.FromMilliseconds (milisect_repeat);
			Memory = new MemoryGrid (ControlledUnidad);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Inteligencia.HumanIntelligence"/> class.
		/// </summary>
		/// <param name="yo">Controlled unidad</param>
		/// <param name = "repTime">Tiempo de repetición</param>
		public HumanIntelligence (Unidad yo, TimeSpan repTime)
		{
			ControlledUnidad = yo;
			MinRepetitionTime = repTime;
			Memory = new MemoryGrid (ControlledUnidad);
		}
	}
}