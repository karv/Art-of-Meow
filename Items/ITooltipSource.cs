
namespace Items
{
	/// <summary>
	/// This object can return the <see cref="string"/> describing itself
	/// </summary>
	public interface ITooltipSource
	{
		/// <summary>
		/// Gets the tooltip info (shown in equipment screen)
		/// </summary>
		string GetTooltipInfo ();
	}
}