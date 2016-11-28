
namespace Skills
{
	/// <summary>
	/// Resultado de un efecto
	/// </summary>
	public enum EffectResultEnum
	{
		/// <summary>
		/// El efecto aún no da resultado
		/// </summary>
		NotInstanced = 0,
		/// <summary>
		/// El efecto falla
		/// </summary>
		Miss,
		/// <summary>
		/// El efecto acierta
		/// </summary>
		Hit
	}
}