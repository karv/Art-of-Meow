using System;

namespace Units.Recursos
{
	/// <summary>
	/// Recurso de valor 'fijo'
	/// </summary>
	public class RecursoEstático : IRecurso
	{
		void AoM.IInternalUpdate.Update (float gameTime)
		{
		}

		System.Collections.Generic.IEnumerable<IParámetroRecurso> IRecurso.EnumerateParameters ()
		{
			throw new Exception ("Cannot get value of this kind of resource.");
		}

		IParámetroRecurso IRecurso.ValorParámetro (string paramName)
		{
			throw new Exception ("Cannot get value of this kind of resource.");
		}

		/// <summary>
		/// Nombre corto
		/// </summary>
		/// <value>The nombre corto.</value>
		public string NombreCorto { get; set; }

		/// <summary>
		/// Nombre largo
		/// </summary>
		/// <value>The nombre largo.</value>
		public string NombreLargo { get; set; }

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		/// <value>The nombre único.</value>
		public string NombreÚnico { get; }

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		/// <value>The valor.</value>
		public float Valor { get; set; }

		/// <summary>
		/// Unidad que posee este recurso.
		/// </summary>
		/// <value>The unidad.</value>
		public IUnidad Unidad { get; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Recursos.RecursoEstático"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Recursos.RecursoEstático"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[RecursoEstático: NombreÚnico={0}, Valor={1}, Unidad={2}]",
				NombreÚnico,
				Valor,
				Unidad);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Recursos.RecursoEstático"/> class.
		/// </summary>
		/// <param name="nombre">Nombre único.</param>
		/// <param name="unidad">Unidad.</param>
		public RecursoEstático (string nombre, IUnidad unidad)
		{
			NombreÚnico = nombre;
			Unidad = unidad;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Recursos.RecursoEstático"/> class.
		/// </summary>
		/// <param name="nombre">Nombre único.</param>
		/// <param name="unidad">Unidad.</param>
		/// <param name="valor">Valor.</param>
		public RecursoEstático (string nombre, IUnidad unidad, float valor)
			: this (nombre, unidad)
		{
			Valor = valor;
		}
	}
}