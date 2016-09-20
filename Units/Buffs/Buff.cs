namespace Units.Buffs
{
	public abstract class Buff : IBuff
	{
		public virtual void Update (float gameTime)
		{
		}

		public abstract bool IsVisible { get; }

		public abstract string BaseTextureName { get; }

		public abstract string Nombre { get; }

		public BuffManager Manager { get; private set; }

		public virtual void Terminating ()
		{
		}

		public virtual void Initialize ()
		{
		}

		BuffManager IBuff.Manager
		{
			get { return Manager; }
			set { Manager = value; }
		}
	}
}