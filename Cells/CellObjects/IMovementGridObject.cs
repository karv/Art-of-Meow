
namespace Cells.CellObjects
{
	/// <summary>
	/// Represents a <see cref="IGridObject"/> that has a determined movement speed
	/// </summary>
	public interface IMovementGridObject : IGridObject
	{
		/// <summary>
		/// Representa el costo base de tiempo para un movimiento desde o hasta este objeto
		/// </summary>
		float CoefMovement { get; }
	}
}