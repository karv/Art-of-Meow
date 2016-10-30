using System;
using AoM;

namespace Helper
{
	/// <summary>
	/// Representa una clase que realiza una acción al pasar un tiempo dado.
	/// </summary>
	public class Timer : IInternalUpdate
	{
		/// <summary>
		/// Tiempo inicial de cada instancia de coundown por esta clase.
		/// </summary>
		/// <value>The conteo inicial.</value>
		public float ConteoInicial { get; set; }

		float _restante;

		/// <summary>
		/// Tiempo restante para cero
		/// </summary>
		/// <value>The restante.</value>
		public float Restante
		{
			get{ return Math.Max (0, _restante); }
			set { _restante = value; }
		}

		/// <summary>
		/// Devuelve o establece si debe reestablecer el contador cada vez que llegue a cero
		/// </summary>
		public bool AutoReset { get; set; }

		/// <summary>
		/// Devuelve un valor indicando que ya se llegó a cero
		/// </summary>
		public bool ZeroIsReached { get { return _restante <= 0; } }

		/// <summary>
		/// Reestablece el contador al valor inicial.
		/// </summary>
		public void Reset ()
		{
			Restante = ConteoInicial;
		}

		/// <summary>
		/// Determina si este control tiene habilitado la invocación a <see cref="Update"/>
		/// </summary>
		public bool Habilitado { get; set; }

		/// <summary>
		/// Updates the object
		/// </summary>
		/// <param name="gameTime">Time to pass</param>
		public void Update (float gameTime)
		{
			if (!Habilitado)
				return;
			if (ConteoInicial <= 0 && AutoReset)
				throw new InvalidOperationException ("Cannot Update if ConteoInicial is non-positive.");
			_restante -= gameTime;
			if (ZeroIsReached)
			{
				conteoCero ();
				if (AutoReset)
				{
					var acumulado = -_restante;
					Reset ();
					Update (acumulado);
				}
			}
		}

		void conteoCero ()
		{
			ConteoCero?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// </summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Helper.Timer"/>.
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Helper.Timer"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[Timer: ConteoInicial={0}, Restante={1}, Habilitado={2}]",
				ConteoInicial,
				Restante,
				Habilitado);
		}

		/// <summary>
		/// Ocurre cada vez que se llega a cero.
		/// </summary>
		public event EventHandler ConteoCero;
	}
}