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

		/// <summary>
		/// The total number of types
		/// </summary>
		__total
	}

	/// <summary>
	/// Provee métodos para generar unidades
	/// </summary>
	public class UnidadFactory
	{
		/// <summary>
		/// Devuelve el tablero de mapa actual
		/// </summary>
		public LogicGrid Grid { get; }

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
		/// Construye una unidad dado su tipo
		/// </summary>
		/// <param name="type">Nombre del tipo de la unidad</param>
		public Unidad MakeEnemy (string type)
		{
			for (int i = 0; i < (int)EnemyType.__total; i++)
			{
				var currEnType = (EnemyType)i;
				if (currEnType.ToString () == type)
					return MakeEnemy (currEnType);
			}
			throw new Exception (string.Format ("Enemy type {0} does not exist.", type));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.UnidadFactory"/> class.
		/// </summary>
		public UnidadFactory (LogicGrid grid)
		{
			if (grid == null)
				throw new ArgumentNullException ("grid");
			Grid = grid;
		}
	}
}