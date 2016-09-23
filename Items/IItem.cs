using Microsoft.Xna.Framework;
using Moggle.Controles;

namespace Items
{
	/// <summary>
	/// Representa la instancia de un objeto
	/// </summary>
	public interface IItem : IComponent, IDibujable
	{
		string Nombre { get; }

		string DefaultTextureName { get; }

		Color DefaultColor { get; }
	}
}