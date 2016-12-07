
namespace Helper
{
	/// <summary>
	/// Represents an object which can randomly select an object
	/// </summary>
	public interface IDistribution<T>
	{
		/// <summary>
		/// Picks an item in the distribution
		/// </summary>
		T Pick ();
	}
}