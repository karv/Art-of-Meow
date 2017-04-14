
namespace Units
{
	/// <summary>
	/// This object can gain experience
	/// </summary>
	public interface IExpable
	{
		/// <summary>
		/// Recibe experiencia.
		/// </summary>
		/// <param name="exp">Cantidad de experiencia recibida</param>
		void ReceiveExperience (float exp);

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		string NombreÚnico { get; }
	}
}