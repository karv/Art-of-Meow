using System;
using Cells;
using Cells.CellObjects;

namespace Units.Inteligencia
{
	public abstract class AI : IUnidadController, ICloneable
	{
		/// <summary>
		/// Gets the map grid.
		/// </summary>
		/// <value>The map grid.</value>
		public LogicGrid MapGrid { get { return ControlledUnidad.Grid; } }

		/// <summary>
		/// Gets the controlled unidad
		/// </summary>
		public Unidad ControlledUnidad { get; private set; }

		public void LinkWith (Unidad unidad)
		{
			if (unidad == null)
				throw new ArgumentNullException ("unidad");
			if (ControlledUnidad != null || unidad.Inteligencia != null)
				throw new InvalidOperationException ("Cannot relink IA");

			ControlledUnidad = unidad;
			unidad.Inteligencia = this;
			AfterUnidadLink ();
		}

		public abstract object Clone ();

		protected virtual void AfterUnidadLink ()
		{
		}

		void checks ()
		{
			ControlledUnidad.assertIsIdle ();
		}

		protected abstract void DoAction ();

		protected bool IsSelectableAsTarget (IGridObject obj)
		{
			var otro = obj as IUnidad;
			return otro != null && ControlledUnidad.IsEnemyOf (otro);
		}

		public static AI GetAIByName (string name)
		{
			switch (name)
			{
				case "Chase":
					return new ChaseIntelligence ();
				case "Ranged":
					return new RangedIntelligence ();
				default:
					throw new Exception ();
			}
		}

		void IUnidadController.DoAction ()
		{
			checks ();
			DoAction ();
		}
	}
}