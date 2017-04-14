﻿using System;
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

namespace Units
{
	/// <summary>
	/// Represents a unit in the game grid
	/// </summary>
	public interface IUnidad :
	IUpdateGridObject,
	ICollidableGridObject,
	IExpGiver,
	IEffectAgent,
	ITarget,
	IGridMoveable,
	IDisposable
	{
		/// <summary>
		/// Gets the map grid
		/// </summary>
		LogicGrid MapGrid { get; }

		/// <summary>
		/// Gets the resources of this unit
		/// </summary>
		/// <value>The recursos.</value>
		ManejadorRecursos Recursos { get; }

		/// <summary>
		/// Gets the equipment of this unit
		/// </summary>
		/// <value>The equipment.</value>
		EquipmentManager Equipment { get; }

		/// <summary>
		/// Devuelve el manejador de experiencia.
		/// </summary>
		ExpManager Exp { get; }

		/// <summary>
		/// Gets the buffs if this unit
		/// </summary>
		/// <value>The buffs.</value>
		BuffManager Buffs { get; }

		/// <summary>
		/// Gets the skills of this unit
		/// </summary>
		/// <value>The skills.</value>
		SkillManager Skills { get; }

		/// <summary>
		/// Enqueues a primitive order
		/// </summary>
		/// <param name="order">Order.</param>
		void EnqueueOrder (IPrimitiveOrder order);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Units.IUnidad"/> is enabled, and therefore may act.
		/// </summary>
		bool Habilitado { get; }

		/// <summary>
		/// Gets the id of the team of this unit
		/// </summary>
		/// <value>The equipo.</value>
		TeamManager Team { get; }

		/// <summary>
		/// Gets the inventory of this unit
		/// </summary>
		IInventory Inventory { get; }
	}

	static class UnidadImplementation
	{
		public static void EnqueueOrder (this IUnidad unid,
		                                 IEnumerable<IPrimitiveOrder> orders)
		{
			foreach (var o in orders)
				unid.EnqueueOrder (o);
		}

		public static bool IsEnemyOf (this IUnidad unid, IUnidad other)
		{
			return !unid.Team.Equals (other.Team);
		}

		public static IEnumerable<ISkill> EnumerateEquipmentSkills (this IUnidad u)
		{
			foreach (var eq in u.Equipment.EnumerateEquipment ().OfType<ISkillEquipment> ())
				foreach (var sk in eq.GetEquipmentSkills ())
					yield return sk;
			
		}

		public static IEnumerable<ISkill> EnumerateInventorySkills (this IUnidad u)
		{
			return u.Inventory.Items.OfType<ISkill> ();
		}

		/// <summary>
		/// Enumera absolutamente todos los skills (inc invisibles y no castables)
		/// de una unidad. 
		/// Esto incluye skill de unidad, de equipment y de inventory
		/// </summary>
		public static IEnumerable<ISkill> EnumerateAllSkills (this IUnidad u)
		{
			foreach (var sk in u.Skills.Skills)
				yield return sk;
			foreach (var sk in EnumerateEquipmentSkills (u))
				yield return sk;
			foreach (var sk in u.EnumerateInventorySkills ())
				yield return sk;
		}

		public static bool CanSee (this IUnidad unid, Point p)
		{
			var visLen = (int)Math.Ceiling (unid.Recursos.ValorRecurso (ConstantesRecursos.Visión));
			var sqVisLen = visLen * visLen;

			return (Geometry.SquaredEucludeanDistance (unid.Location, p) <= sqVisLen &&
			unid.Grid.IsVisibleFrom (unid.Location, p));
		}

		public static IEnumerable<Point> VisiblePoints (this IUnidad unid)
		{
			var visLen = (int)Math.Ceiling (unid.Recursos.ValorRecurso (ConstantesRecursos.Visión));
			var posRect = new Rectangle (unid.Location.X - visLen,
				              unid.Location.Y - visLen,
				              2 * visLen, 2 * visLen);

			return posRect.EnumeratePoints ().Where (unid.CanSee);
		}
	}
}