using System.Collections.Generic;
using AoM;
using Moggle.Controles;
using Units;

namespace Units.Recursos
{
	/// <summary>
	/// Un recurso que puede ser mostrado en <see cref="Componentes.RecursoView"/>
	/// </summary>
	public interface IVisibleRecurso : IRecurso, IDibujable
	{
		/// <summary>
		/// El recurso es visible
		/// </summary>
		bool Visible { get; }
	}

	/// <summary>
	/// Un 'stat' de una unidad.
	/// </summary>
	public interface IRecurso : IInternalUpdate
	{
		/// <summary>
		/// Nombre corto
		/// </summary>
		string NombreCorto { get; }

		/// <summary>
		/// Nombre largo
		/// </summary>
		string NombreLargo { get; }

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		string NombreÚnico { get; }

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		float Valor { get; set; }

		/// <summary>
		/// Devuelve el valor de un parámetro
		/// </summary>
		/// <returns>The parámetro.</returns>
		/// <param name="paramName">Parameter name.</param>
		IParámetroRecurso ValorParámetro (string paramName);

		/// <summary>
		/// Enumera los parámetros de este recurso
		/// </summary>
		/// <returns>The parameters.</returns>
		IEnumerable<IParámetroRecurso> EnumerateParameters ();

		/// <summary>
		/// Unidad que posee este recurso.
		/// </summary>
		IUnidad Unidad { get; }
	}

	/// <summary>
	/// Representa un parámetro de un <see cref="IRecurso"/>
	/// </summary>
	public interface IParámetroRecurso : IInternalUpdate
	{
		/// <summary>
		/// Gets the <see cref="Recurso"/> that has this parameter
		/// </summary>
		IRecurso Recurso { get; }

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		string NombreÚnico { get; }

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		float Valor { get; set; }

		/// <summary>
		/// Recibe experiencia.
		/// </summary>
		/// <param name="exp">Cantidad de experiencia recibida</param>
		void ReceiveExperience (float exp);
	}
}