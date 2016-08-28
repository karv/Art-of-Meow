using MonoGame.Extended;
using Units;

namespace Units.Recursos
{
	/// <summary>
	/// Un 'stat' de una unidad.
	/// </summary>
	public interface IRecurso : IUpdate
	{
		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		string NombreÚnico { get; }

		/// <summary>
		/// Nombre corto
		/// </summary>
		string NombreCorto { get; }

		/// <summary>
		/// Nombre largo
		/// </summary>
		string NombreLargo { get; }

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		float Valor { get; }

		/// <summary>
		/// Unidad que posee este recurso.
		/// </summary>
		IUnidad Unidad { get; }
	}
}