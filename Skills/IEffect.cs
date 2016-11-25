using Skills;

namespace Skills
{
	/// <summary>
	/// Un efecto de un objeto.
	/// </summary>
	public interface IEffect
	{
		/// <summary>
		/// Quien causa el efecto
		/// </summary>
		IEffectAgent Agent { get; }

		/// <summary>
		/// Probabilidad de que ocurra
		/// </summary>
		double Chance { get; }

		/// <summary>
		/// Runs the effect
		/// </summary>
		void Execute ();

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		string DetailedInfo ();
	}
}