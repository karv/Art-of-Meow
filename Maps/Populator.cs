using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Cells;
using Cells.CellObjects;
using Helper;
using Items;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Units;

namespace Maps
{
	/// <summary>
	/// Provides methods to populate and repopulate a map.
	/// </summary>
	public class Populator
	{
		internal readonly List<PopulationRule> Rules;

		public Populator ()
		{
			Rules = new List<PopulationRule> ();
		}
	}
	
}