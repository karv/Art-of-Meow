using Moggle.Controles;

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
}