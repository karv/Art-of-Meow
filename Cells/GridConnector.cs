using System.Collections.Generic;
using AoM;
using Microsoft.Xna.Framework;
using Maps;
using Cells.CellObjects;

namespace Cells
{
	/// <summary>
	/// Controla las conexiones topológicas de un <see cref="LogicGrid"/> origen
	/// </summary>
	public class GridConnector
	{
		Dictionary <Point, WorldLocation> Connections { get; }

		List<Point> PendingConnections { get; }

		LogicGrid Grid { get; }

		/// <summary>
		/// Agrega una conexión determinando el punto final
		/// </summary>
		public void AddConnection (Point originPoint, WorldLocation endPoint)
		{
			Connections.Add (originPoint, endPoint);
		}

		/// <summary>
		/// Agrega una conexión, dejando el punto final sin asignar
		/// </summary>
		public void AddConnection (Point originPoint)
		{
			PendingConnections.Add (originPoint);
		}

		/// <summary>
		/// Devuelve el punto final que le corresponte a un punto origen dado
		/// </summary>
		/// <returns>El punto final cuando exista; en caso contrario es <c>null</c></returns>
		/// <param name="p">Punto origen en este tablero</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.GridConnector"/> class.
		/// </summary>
		public GridConnector (LogicGrid grid)
		{
			Connections = new Dictionary<Point, WorldLocation> ();
			PendingConnections = new List<Point> ();
			Grid = grid;
		}
	}
}