using System;
using AdvMath;

namespace Units.Recursos
{
	class RecFml : Fórmula
	{
		readonly ManejadorRecursos _manRec;

		public override float EvaluarVariable (string nombreVariable)
		{
			var ret = _manRec.ValorRecurso (nombreVariable);

			if (ret.HasValue)
				return ret.Value;
			throw new Exception ("Recurso inexistente.");
		}

		public RecFml (string s, ManejadorRecursos man)
			: base (s)
		{
			_manRec = man;
		}
	}

	public class RecursoFml : IRecurso
	{
		/// <summary>
		/// Fórmula del recurso
		/// </summary>
		/// <value>The fml.</value>
		public string Fórmula { get { return  Fml.StrFormula; } }

		protected Fórmula Fml { get; }

		#region IInternalUpdate implementation

		public void Update (float gameTime)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IRecurso implementation

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		/// <value>The nombre único.</value>
		public string NombreÚnico { get; }

		/// <summary>
		/// Nombre corto
		/// </summary>
		/// <value>The nombre corto.</value>
		public string NombreCorto { get; set; }

		/// <summary>
		/// Nombre largo
		/// </summary>
		/// <value>The nombre largo.</value>
		public string NombreLargo { get; set; }

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		/// <value>The valor.</value>
		public float Valor
		{
			get { return Fml.Evaluar (); }
			set { throw new InvalidOperationException (); }
		}

		public IUnidad Unidad { get; }

		#endregion

		/// <summary>
		/// </summary>
		/// <param name="fml">Fórmula del valor</param>
		/// <param name="unidad">Unidad</param>
		/// <param name="nombre">Nombre único</param>
		public RecursoFml (string fml, IUnidad unidad, string nombre)
		{
			Fml = new RecFml (fml, unidad.Recursos);
			Unidad = unidad;
			NombreÚnico = nombre;
		}
	}
}