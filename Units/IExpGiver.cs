
namespace Units
{
	/// <summary>
	/// Representa un objeto que puede otorgar experiencia.
	/// </summary>
	public interface IExpGiver
	{
		/// <summary>
		/// Calcula la experiencia que este objeto da
		/// </summary>
		float GetExperienceValue ();
	}
}