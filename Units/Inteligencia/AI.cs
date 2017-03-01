using System;
using Cells;
using Cells.CellObjects;

namespace Units.Inteligencia
{
	/// <summary>
	/// The artifical intelligence common abstract class
	/// </summary>
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

		/// <summary>
		/// Links the IA with a unidad
		/// </summary>
		/// <param name="unidad">Unidad.</param>
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

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public abstract object Clone ();

		/// <summary>
		/// This method is invoked after <see cref="LinkWith"/>
		/// </summary>
		protected virtual void AfterUnidadLink ()
		{
		}

		void checks ()
		{
			ControlledUnidad.assertIsIdle ();
		}

		/// <summary>
		/// Do the intelligence part.
		/// This is invoked every grid update to control the will of <see cref="ControlledUnidad"/>
		/// </summary>
		protected abstract void DoAction ();

		/// <summary>
		/// Determines if a grid object may be selected as a target
		/// </summary>
		/// <param name="obj">The grid object</param>
		protected bool IsSelectableAsTarget (IGridObject obj)
		{
			var otro = obj as IUnidad;
			return otro != null && ControlledUnidad.IsEnemyOf (otro);
		}

		/// <summary>
		/// Gets a copy on an IA
		/// </summary>
		/// <param name="name">The name of the IA</param>
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