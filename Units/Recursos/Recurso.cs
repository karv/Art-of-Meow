using System;
using System.Collections.Generic;

namespace Units.Recursos
{
	/// <summary>
	/// A common implementation for <see cref="IRecurso"/>
	/// </summary>
	public abstract class Recurso : IRecurso
	{
		/// <summary>
		/// The parameters
		/// </summary>
		protected List<IParámetroRecurso> Parámetros;

		/// <summary>
		/// Enumera los parámetros de este recurso
		/// </summary>
		public IEnumerable<IParámetroRecurso> EnumerateParameters ()
		{
			return Parámetros;
		}

		#region IRecurso implementation

		/// <summary>
		/// Gets the value of a parameter
		/// </summary>
		/// <returns>The parámetro.</returns>
		/// <param name="paramName">Parameter name.</param>
		public IParámetroRecurso ValorParámetro (string paramName)
		{
			foreach (var p in Parámetros)
				if (p.NombreÚnico == paramName)
					return p;
			throw new Exception ();
		}

		/// <summary>
		/// Gets the value of the entire Recurso
		/// </summary>
		/// <value>The valor.</value>
		public abstract float Valor { get; set; }

		/// <summary>
		/// Gets the short name
		/// </summary>
		protected abstract string GetShortName ();

		/// <summary>
		/// Gets the detailed name
		/// </summary>
		protected abstract string GetLongName ();

		/// <summary>
		/// Gets the unique name
		/// </summary>
		protected abstract string GetUniqueName ();

		string IRecurso.NombreCorto { get { return GetShortName (); } }

		string IRecurso.NombreLargo { get { return GetLongName (); } }

		string IRecurso.NombreÚnico { get { return GetUniqueName (); } }

		/// <summary>
		/// Gets the <see cref="IUnidad"/> that has this <see cref="IRecurso"/>
		/// </summary>
		public IUnidad Unidad { get; }

		#endregion

		#region IInternalUpdate implementation

		/// <summary>
		/// Updates the parameters
		/// </summary>
		public virtual void Update (float gameTime)
		{
			foreach (var par in Parámetros)
				par.Update (gameTime);
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Recursos.Recurso"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Recursos.Recurso"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[Recurso: Parámetros={0}, Valor={1}, NombreÚnico={2}, Unidad={3}]",
				Parámetros,
				Valor,
				GetUniqueName (),
				Unidad);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Recursos.Recurso"/> class.
		/// </summary>
		/// <param name="unidad">Unidad that has this recurso</param>
		protected Recurso (IUnidad unidad)
		{
			Unidad = unidad;
			Parámetros = new List<IParámetroRecurso> ();
		}
	}
}