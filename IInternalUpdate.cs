namespace AoM
{
	/// <summary>
	/// An object that can be updated by the <see cref="GameTimeManager"/>
	/// </summary>
	public interface IInternalUpdate
	{
		/// <summary>
		/// Updates the object
		/// </summary>
		void Update (float gameTime);
	}
}