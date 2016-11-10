namespace Units.Buffs
{
	/// <summary>
	/// A common implementation of a <see cref="IBuff"/>
	/// </summary>
	public abstract class Buff : IBuff
	{
		/// <summary>
		/// Updates the buff
		/// </summary>
		/// <param name="gameTime">Time passed since last update</param>
		public virtual void Update (float gameTime)
		{
		}

		/// <summary>
		/// Gets a value indicating if this buff is shown in the list of active buffs
		/// </summary>
		public abstract bool IsVisible { get; }

		/// <summary>
		/// Devuelve la textura a usar
		/// </summary>
		public abstract string BaseTextureName { get; }

		/// <summary>
		/// Name of the buff
		/// </summary>
		public abstract string Nombre { get; }

		/// <summary>
		/// El manejador de buffs
		/// </summary>
		/// <value>The manager.</value>
		public BuffManager Manager { get; private set; }

		/// <summary>
		/// Se invoca cuando está por desanclarse.
		/// No hace nada
		/// </summary>
		public virtual void Terminating ()
		{
		}

		/// <summary>
		/// De invoca justo después de anclarse.
		/// No hace nada
		/// </summary>
		public virtual void Initialize ()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Buffs.Buff"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Buffs.Buff"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[Buff: Nombre={0}, Unidad={1}]",
				Nombre,
				Manager.HookedOn);
		}


		BuffManager IBuff.Manager
		{
			get { return Manager; }
			set { Manager = value; }
		}
	}
}