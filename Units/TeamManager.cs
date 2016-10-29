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

		public Color TeamColor { get; set; }

		public TeamManager ()
		{
			_members = new List<IUnidad> ();
		}
	}
}