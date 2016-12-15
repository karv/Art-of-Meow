using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Cells.CellObjects;
using Cells.Collision;
using Helper;
using Items;
using Microsoft.Xna.Framework;
using Skills;
using Units.Buffs;
using Units.Equipment;
using Units.Order;
using Units.Recursos;
using Units.Skills;

namespace Units
{
	public interface IGridMoveable : IGridObject
	{
		/// <summary>
		/// Determines whether this instance can move to the specified destination.
		/// </summary>
		bool CanMove (Point destination);

		/// <summary>
		/// Executed before any movement
		/// </summary>
		void BeforeMoving (Point destination);

		/// <summary>
		/// Executes after any movement
		/// </summary>
		void AfterMoving (Point destination);

		/// <summary>
		/// Ocurrs when location changes
		/// </summary>
		event EventHandler OnRelocation;
	}
	
}