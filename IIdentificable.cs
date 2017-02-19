
namespace AoM
{
	/// <summary>
	/// Represents an object that may be identified with a unique name
	/// </summary>
	public interface IIdentificable
	{
		/// <summary>
		/// Gets the unique name
		/// </summary>
		string Name { get; }
	}
}