using System;
using System.Collections.Generic;
using System.Linq;
using AoM;

namespace Units.Buffs
{
	/// <summary>
	/// Manejador de buffs
	/// </summary>
	public class BuffManager : IInternalUpdate
	{
		/// <summary>
		/// Devuelve la unidad donde estos buffs están anclados.
		/// </summary>
		/// <value>The hooked on.</value>
		public IUnidad HookedOn { get; }

		List<IBuff> Buffs { get; }

		/// <summary>
		/// Devuelve el número de buffs
		/// </summary>
		/// <value>The count.</value>
		public int Count { get { return Buffs.Count; } }

		/// <summary>
		/// Enumera los buffs
		/// </summary>
		public IEnumerable<IBuff> Enumerate ()
		{
			return Buffs;
		}

		/// <summary>
		/// Agrega un buff
		/// </summary>
		public void Hook (IBuff buff)
		{
			if (buff == null)
				throw new ArgumentNullException ("buff");
			if (buff.Manager != null)
				throw new InvalidOperationException ("Buff already hooked.");
			buff.Manager = this;
			Buffs.Add (buff);
			buff.Initialize ();
			AddBuff?.Invoke (this, buff);
			ListChanged?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Elimina un hook
		/// </summary>
		public void UnHook (IBuff buff)
		{
			if (buff == null)
				throw new ArgumentNullException ("buff");
			if (buff.Manager != this)
				throw new InvalidOperationException ("Buff is not hooked.");
			RemoveBuff?.Invoke (this, buff);
			ListChanged?.Invoke (this, EventArgs.Empty);
			buff.Terminating ();
			buff.Manager = null;
			Buffs.Remove (buff);
		}

		/// <summary>
		/// Determina si tiene un buff dado
		/// </summary>
		/// <returns><c>true</c> si tiene el buff; otherwise, <c>false</c>.</returns>
		/// <param name="buff">Buff.</param>
		public bool HasBuff (IBuff buff)
		{
			return Buffs.Contains (buff);
		}

		/// <summary>
		/// Determina si tiene un buff dado
		/// </summary>
		/// <returns><c>true</c> si tiene el buff; otherwise, <c>false</c>.</returns>
		/// <param name="nombreÚnico">nombre único del buff</param>
		public bool HasBuff (string nombreÚnico)
		{
			return Buffs.Any (z => z.Nombre == nombreÚnico);
		}

		/// <summary>
		/// Determina si contiene un buff de un tipo dado.
		/// </summary>
		public bool HasBuffOfType<T> ()
			where T : IBuff
		{
			return BuffOfType<T> ().Any ();
		}

		/// <summary>
		/// Enumera los buffs de un cierto tipo
		/// </summary>
		public IEnumerable<T> BuffOfType<T> ()
		{
			return Buffs.OfType<T> ();
		}

		/// <summary>
		/// Ocurre cuando hay un nuevo hook
		/// </summary>
		public event EventHandler<IBuff> AddBuff;
		/// <summary>
		/// Ocurre cuando se elimina o termina un hook
		/// </summary>
		public event EventHandler<IBuff> RemoveBuff;

		/// <summary>
		/// Occurs when the collection changed
		/// </summary>
		public event EventHandler ListChanged;

		/// <summary>
		/// Updates the buffs
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void Update (float gameTime)
		{
			foreach (var buff in Buffs)
				buff.Update (gameTime);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Buffs.BuffManager"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Buffs.BuffManager"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[BuffManager: HookedOn={0}, Buffs={1}, Count={2}]",
				HookedOn,
				Buffs,
				Count);
		}

		/// <summary>
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		public BuffManager (IUnidad unidad)
		{
			HookedOn = unidad;
			Buffs = new List<IBuff> ();
			Buffs.Add (unidad.Equipment.EquipBuff);
		}
	}
}