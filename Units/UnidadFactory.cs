using System;
using System.Collections.Generic;
using System.Diagnostics;
using AoM;
using Cells;
using Debugging;
using Items;
using Moggle;
using Units.Recursos;

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
	/// La clase de un enemigo para su creación
	/// </summary>
	public enum EnemyClass
	{
		/// <summary>
		/// Clase warrior
		/// </summary>
		Warrior,
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
		/// Gets the content manager
		/// </summary>
		protected static BibliotecaContenido Content { get { return Program.MyGame.Contenido; } }

		static Random r = new Random ();

		/// <summary>
		/// Muestra toda la información de una unidad por el debug
		/// </summary>
		/// <param name="u">Unidad</param>
		[Conditional ("DEBUG")]
		public static void DebugAllInfo (Unidad u)
		{
			Debug.WriteLine (
				string.Format ("Unidad creada: {0}", u.Nombre),
				DebugCategories.UnitFactory);
			foreach (var re in u.Recursos.Enumerate ())
			{
				foreach (var pa in re.EnumerateParameters ())
				{
					Debug.Write (
						string.Format (
							"{2}.{0}: {1}\t",
							pa.NombreÚnico,
							pa.Valor,
							re.NombreÚnico),
						DebugCategories.UnitFactory);
				}
				Debug.WriteLine ("", DebugCategories.UnitFactory);
			}
		}

		static string getTextureString (EnemyType enemyType)
		{
			switch (enemyType)
			{
				case EnemyType.Imp:
					return "swordman";
				default:
					return "swordman";
			}
		}

		/// <summary>
		/// Construye una unidad dado su tipo
		/// </summary>
		/// <param name="enemyType">Tipo de unidad</param>
		/// <param name = "enemyClass">Clase de la unidad</param>
		/// <param name = "exp">Experiencia total de la unidad</param>
		public Unidad MakeEnemy (EnemyType enemyType,
		                         EnemyClass enemyClass,
		                         float exp = 0)
		{
			var textureString = getTextureString (enemyType);
			var ret = new Unidad (Grid, textureString);

			foreach (var x in GetAttrDistribution (enemyType, enemyClass))
				ret.Exp.AddAssignation (x.Key, x.Value);
			ret.Exp.ExperienciaAcumulada = exp;
			ret.Exp.Flush ();

			ret.RecursoHP.Fill ();
			ret.Inteligencia = new Inteligencia.ChaseIntelligence (ret);
			ret.Nombre = enemyType.ToString ();

			// Drops
			if (r.NextDouble () < 0.2)
				ret.Inventory.Add (ItemFactory.CreateItem (ItemType.LeatherCap));
			if (r.NextDouble () < 0.1)
				ret.Inventory.Add (ItemFactory.CreateItem (ItemType.LeatherArmor));
			if (r.NextDouble () < 0.4)
				ret.Inventory.Add (ItemFactory.CreateItem (ItemType.HealingPotion));

			DebugAllInfo (ret);
			Debug.WriteLine (string.Format (
				"Stats para unidad creada {0}:", ret.Nombre),
				DebugCategories.UnitFactory);
			return ret;
		}

		/// <summary>
		/// Devuelve una distribución de experiencia normalizada para una clase dada
		/// </summary>
		public Dictionary<string, float> GetAttrDistribution (EnemyClass eClass)
		{
			var ret = new Dictionary<string,float> ();
			switch (eClass)
			{
				case EnemyClass.Warrior: // 1.25
					ret.Add (ConstantesRecursos.CertezaMelee, 0.16f);
					ret.Add (ConstantesRecursos.Destreza, 0.16f);
					ret.Add (ConstantesRecursos.EvasiónMelee, 0.08f);
					ret.Add (ConstantesRecursos.EvasiónRango, 0.04f);
					ret.Add (ConstantesRecursos.Fuerza, 0.24f);
					ret.Add (ConstantesRecursos.HP, 0.16f);
					ret.Add (ConstantesRecursos.Velocidad, 0.08f);
					break;
				default:
					throw new Exception ();
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una distribución de experiencia normalizada para una raza dada
		/// </summary>
		public Dictionary<string, float> GetAttrDistribution (EnemyType eType)
		{
			var ret = new Dictionary<string,float> ();
			switch (eType)
			{
				case EnemyType.Imp:
					ret.Add (ConstantesRecursos.CertezaMelee, 1f);
					ret.Add (ConstantesRecursos.Destreza, 0.7f);
					ret.Add (ConstantesRecursos.EvasiónMelee, 1f);
					ret.Add (ConstantesRecursos.EvasiónRango, 0.8f);
					ret.Add (ConstantesRecursos.Fuerza, 0.2f);
					ret.Add (ConstantesRecursos.HP, 0.4f);
					ret.Add (ConstantesRecursos.Velocidad, 0.4f);
					break;
				default:
					throw new Exception ();
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una distribución de experiencia normalizada para clase y raza dadas, y una función de peso para la raza
		/// </summary>
		public Dictionary<string, float> GetAttrDistribution (EnemyType eType,
		                                                      EnemyClass eClass,
		                                                      float typeWeight = 0.5f)
		{
			if (typeWeight < 0 || typeWeight > 1)
				throw new ArgumentOutOfRangeException (
					"typeWeight",
					"typeWeight must be a non-negative number at most 1");
			
			var ret = new Dictionary<string,float> ();
			foreach (var assign in GetAttrDistribution (eType))
				ret.Add (assign.Key, assign.Value * typeWeight);
			
			float classWeight = 1 - typeWeight;
			foreach (var assign in GetAttrDistribution (eClass))
			{
				float prevVal;
				if (ret.TryGetValue (assign.Key, out prevVal))
					ret [assign.Key] = prevVal + assign.Value * classWeight;
				else
					ret.Add (assign.Key, assign.Value * classWeight);
			}
			return ret;
		}


		/// <summary>
		/// Construye una unidad dado su tipo
		/// </summary>
		/// <param name="type">Nombre del tipo de la unidad</param>
		/// <param name = "class">Clase de la unidad</param>
		/// <param name = "exp">Experiencia total de la unidad</param>
		public Unidad MakeEnemy (string type, string @class, float exp = 0)
		{
			EnemyType eType = EnemyType.__total; // Para saber que no está asignado
			EnemyClass eClass = EnemyClass.__total;

			for (int i = 0; i < (int)EnemyType.__total; i++)
			{
				var currEnType = (EnemyType)i;
				if (currEnType.ToString () == type)
					eType = currEnType;
			}
			for (int i = 0; i < (int)EnemyClass.__total; i++)
			{
				var currEnClass = (EnemyClass)i;
				if (currEnClass.ToString () == @class)
					eClass = currEnClass;
			}

			Debug.Assert (eClass != EnemyClass.__total);
			Debug.Assert (eType != EnemyType.__total);

			return MakeEnemy (eType, eClass, exp);
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