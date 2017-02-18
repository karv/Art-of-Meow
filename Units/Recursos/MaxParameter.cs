using System;
using System.Diagnostics;

namespace Units.Recursos
{
	/// <summary>
	/// Represents a upperbounded parameter
	/// </summary>
	public abstract class MaxParameter : IParámetroRecurso
	{
		float value;
		float max;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public float Value
		{
			get { return value; }
			set
			{
				var before = Value;
				this.value = Math.Min (Math.Max (value, 0), Max);

				onValueChanged (before);
			}
		}

		/// <summary>
		/// Gets or sets the max value
		/// </summary>
		public virtual float Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
				if (max < Value)
				{
					Debug.WriteLine ("Max set < value");
					Value = max;
				}
			}
		}

		void onValueChanged (float oldValue)
		{
			// Si no hay cambio, regresar inmediatamente
			if (Value == oldValue)
				return;
			ValorChanged?.Invoke (this, oldValue);

			if (Value == Max)
				ReachedMax?.Invoke (this, EventArgs.Empty);
			if (Value == 0)
				ReachedZero?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Ocurre cuando su valor cambia,
		/// su argumento dice su valor antes del cambio
		/// </summary>
		public event EventHandler<float> ValorChanged;
		/// <summary>
		/// Occurs when reached zero.
		/// </summary>
		public event EventHandler ReachedZero;
		/// <summary>
		/// Occurs when reached max.
		/// </summary>
		public event EventHandler ReachedMax;

		#region Parameter abstract

		/// <summary>
		/// Receives experience
		/// </summary>
		/// <param name="exp">Experience points received</param>
		public abstract void ReceiveExperience (float exp);

		/// <summary>
		/// Updates the object
		/// </summary>
		public virtual void Update (float gameTime)
		{
		}

		/// <summary>
		/// Gets the resource which ahs this parameter
		/// </summary>
		public IRecurso Recurso { get; }

		/// <summary>
		/// Gets the unique name for this resource
		/// </summary>
		public abstract string NombreÚnico { get; }

		float IParámetroRecurso.Valor
		{
			get { return Value; }
			set { Value = value; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Recursos.MaxParameter"/> class.
		/// </summary>
		/// <param name="resource">Resource</param>
		protected MaxParameter (IRecurso resource)
		{
			if (resource == null)
				throw new ArgumentNullException ("resource");
			Recurso = resource;
		}
	}
}