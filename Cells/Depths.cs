using System;


namespace Cells
{
	/// <summary>
	/// A list of constants values of depth drawing
	/// </summary>
	public static class Depths
	{
		/// <summary>
		/// The background object depth
		/// </summary>
		public const float Background = 1f;
		/// <summary>
		/// The ground decoration depth
		/// </summary>
		public const float GroundDecoration = 0.9f;
		/// <summary>
		/// The foreground objects depth
		/// </summary>
		public const float Foreground = 0.15f;
		/// <summary>
		/// Unit's depth
		/// </summary>
		public const float Unit = 0.1f;
		/// <summary>
		/// The controls layer
		/// </summary>
		public const float Gui = 0;

		#region GuiSkillIcon

		public const float SkillIcon = 0.05f;

		#endregion
	}
}