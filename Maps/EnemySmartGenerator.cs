using System;
using System.Collections.Generic;
using Cells;
using Helper;
using Microsoft.Xna.Framework;
using Units;

namespace Maps
{
	/// <summary>
	/// The enemy generator for a <see cref="LogicGrid"/>
	/// </summary>
	[Obsolete ("Populator will do the work now")]
	public class EnemySmartGenerator
	{
		UnidadFactory Factory { get; }

		readonly Random _r;

		LogicGrid grid { get { return Factory.Grid; } }

		readonly IDistribution<int> NumEnemiesInterval;

		static readonly Color enemyColor = Color.Blue;

		List<EnemyGenerationData> enemyType { get; }

		/// <summary>
		/// Add new enemies to it's <see cref="LogicGrid"/>
		/// </summary>
		public void PopulateGrid ()
		{
			var numEnemies = NumEnemiesInterval.Pick ();	// Number of enemies
			var expEach = Difficulty / numEnemies;			// Experience for each enemy

			var enemyTeam = new TeamManager (enemyColor);
			for (int i = 0; i < numEnemies; i++)
			{
				// Pick a data
				var data = enemyType [_r.Next (enemyType.Count)];

				var enemy = Factory.MakeEnemy (data.Type, data.Class, expEach);
				enemy.Team = enemyTeam;

				var point = grid.GetRandomEmptyCell ();
				enemy.Location = point;
				grid [point].Add (enemy);
			}
		}

		/// <summary>
		/// Difficulty of the enemies of the grid.
		/// It is the total received exp for it's enemies.
		/// </summary>
		public float Difficulty;

		/// <summary>
		/// Adds the type of enemy
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="class">Class.</param>
		public void AddEnemyType (EnemyType type, EnemyClass @class)
		{
			enemyType.Add (new EnemyGenerationData (type, @class));
		}

		/// <summary>
		/// Adds the type of enemy.
		/// </summary>
		/// <param name="enemyType">Enemy type.</param>
		public void AddEnemyType (EnemyGenerationData enemyType)
		{
			this.enemyType.Add (enemyType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.EnemySmartGenerator"/> class.
		/// </summary>
		public EnemySmartGenerator (UnidadFactory factory,
		                            float difficulty,
		                            int minEnemies = 0,
		                            int maxEnemines = 5)
		{
			Factory = factory;
			Difficulty = difficulty;
			NumEnemiesInterval = new IntegerInterval (minEnemies, maxEnemines);
			enemyType = new List<EnemyGenerationData> ();
			_r = new Random ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.EnemySmartGenerator"/> class.
		/// </summary>
		public EnemySmartGenerator (LogicGrid grid,
		                            float difficulty,
		                            int minEnemies = 0,
		                            int maxEnemines = 5)
			: this (new UnidadFactory (grid),
			        difficulty,
			        minEnemies,
			        maxEnemines)
		{
		}

		/// <summary>
		/// Has the information to generate a new enemies
		/// </summary>
		public struct EnemyGenerationData
		{
			/// <summary>
			/// Type of enemy
			/// </summary>
			/// <value>The type.</value>
			public EnemyType Type { get; }

			/// <summary>
			/// Class of enemy
			/// </summary>
			/// <value>The class.</value>
			public EnemyClass Class { get; }

			/// <param name="type">Type.</param>
			/// <param name="class">Class.</param>
			public EnemyGenerationData (EnemyType type, EnemyClass @class)
			{
				Type = type;
				Class = @class;
			}
		}
	}
}