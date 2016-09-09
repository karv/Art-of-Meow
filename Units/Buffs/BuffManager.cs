using System.Collections.Generic;
using System;
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
			buff.Terminating ();
			buff.Manager = null;
			Buffs.Remove (buff);
		}

		/// <summary>
		/// Ocurre cuando hay un nuevo hook
		/// </summary>
		public event EventHandler<IBuff> AddBuff;
		/// <summary>
		/// Ocurre cuando se elimina o termina un hook
		/// </summary>
		public event EventHandler<IBuff> RemoveBuff;

		public void Update (float gameTime)
		{
			foreach (var buff in Buffs)
				buff.Update (gameTime);
		}

		/// <summary>
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		public BuffManager (IUnidad unidad)
		{
			HookedOn = unidad;
		}
	}
}