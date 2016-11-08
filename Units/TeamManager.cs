using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Units
{
	/// <summary>
	/// Manages teams properties
	/// </summary>
	public class TeamManager
	{
		List<IUnidad> _members { get; }

		ICollection<IUnidad> Members { get { return _members; } }

		/// <summary>
		/// Gets or sets the color of the team.
		/// </summary>
		/// <value>The color of the team.</value>
		public Color TeamColor { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.TeamManager"/> class.
		/// </summary>
		public TeamManager ()
		{
			_members = new List<IUnidad> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.TeamManager"/> class.
		/// </summary>
		/// <param name="color">Color.</param>
		public TeamManager (Color color)
			: this ()
		{
			TeamColor = color;
		}
	}
}