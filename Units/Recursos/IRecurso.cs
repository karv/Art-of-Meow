using MonoGame.Extended;
using Units;

namespace Units
{
	/// <summary>
	/// Un 'stat' de una unidad.
	/// </summary>
	public interface IRecurso : IUpdate
	{
		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		string Nombre { get; }

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