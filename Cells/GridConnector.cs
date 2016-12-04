using System.Collections.Generic;
using AoM;
using Microsoft.Xna.Framework;
using Maps;
using Cells.CellObjects;

namespace Cells
{
	public class GridConnector
	{
		Dictionary <Point, WorldLocation> Connections { get; }

		List<Point> PendingConnections { get; }

		LogicGrid Grid { get; }

		public void AddConnection (Point originPoint, WorldLocation endPoint)
		{
			Connections.Add (originPoint, endPoint);
		}

		public void AddConnection (Point originPoint)
		{
			PendingConnections.Add (originPoint);
		}

		public WorldLocation? EndPointOf (Point p)
		{
			WorldLocation ret;
			if (Connections.TryGetValue (p, out ret))
				return ret;
			if (PendingConnections.Contains (p))
			{
				var newMap = Map.GetRandomMap ();
				var newGrid = newMap.GenerateGrid ();
				PendingConnections.Remove (p);
				var pt = newGrid.GetRandomEmptyCell ();
				newGrid.AddCellObject (new StairsGridObject (newGrid)
					{ Location = pt });

				// Hacer la conección inversa
				var fromPoint = new WorldLocation (Grid, p);
				newGrid.LocalTopology.AddConnection (pt, fromPoint);

				ret = new WorldLocation (newGrid, pt);
				Connections.Add (p, ret);
				return ret;
			}
			return null;
		}

		public GridConnector (LogicGrid grid)
		{
			Connections = new Dictionary<Point, WorldLocation> ();
			PendingConnections = new List<Point> ();
			Grid = grid;
		}
	}
	
}