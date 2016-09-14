namespace Cells.CellObjects
{
	public interface IMovementGridObject : IGridObject
	{
		/// <summary>
		/// Representa el costo base de tiempo para un movimiento desde o hasta este objeto
		/// </summary>
		float CoefMovement { get; }
	}
}