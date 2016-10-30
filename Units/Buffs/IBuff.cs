using AoM;

namespace Units.Buffs
{
	/// <summary>
	/// Represents a buff, a temporal? state affecting a <see cref="IUnidad"/>
	/// </summary>
	public interface IBuff : IInternalUpdate
	{
		/// <summary>
		/// Nombre
		/// </summary>
		string Nombre { get; }

		/// <summary>
		/// Devuelve la textura a usar
		/// </summary>
		string BaseTextureName { get; }

		/// <summary>
		/// Gets a value indicating if this buff is shown in the list of active buffs
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// El manejador de buffs
		/// </summary>
		/// <value>The manager.</value>
		BuffManager Manager { get; set; }

		/// <summary>
		/// Se invoca cuando está por desanclarse
		/// </summary>
		void Terminating ();

		/// <summary>
		/// De invoca justo después de anclarse.
		/// </summary>
		void Initialize ();
	}
}