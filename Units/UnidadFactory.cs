using System;
using Cells;

namespace Units
{
	/// <summary>
	/// Enumera los tipos de enemigos
	/// </summary>
	public enum EnemyType
	{
		/// <summary>
		/// Un duende, fácil
		/// </summary>
		Imp,
	}

	/// <summary>
	/// Provee métodos para generar unidades
	/// </summary>
	public class UnidadFactory
	{
		/// <summary>
		/// Devuelve el tablero de mapa actual
		/// </summary>
		public Grid Grid { get; }

		/// <summary>
		/// Construye una unidad dado su tipo
		/// </summary>
		/// <param name="enemyType">Tipo de unidad</param>
		public Unidad MakeEnemy (EnemyType enemyType)
		{
			switch (enemyType)
			{
				case EnemyType.Imp:
					var ret = new Unidad (Grid, "swordman");
					ret.RecursoHP.Max = 1;
					ret.RecursoHP.Fill ();
					ret.Inteligencia = new Inteligencia.ChaseIntelligence (ret);
					return ret;
				default:
					throw new NotImplementedException ("Enemy type " + enemyType + " not implemented");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.UnidadFactory"/> class.
		/// </summary>
		public UnidadFactory (Grid grid)
		{
			if (grid == null)
				throw new ArgumentNullException ("grid");
			Grid = grid;
		}
	}
}