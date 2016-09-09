using System.Collections.Generic;
using System.Text;

namespace Units.Recursos
{
	/// <summary>
	/// Clase que sirve como medio entre un <see cref="IUnidad"/> y sus <see cref="IRecurso"/>
	/// </summary>
	public class ManejadorRecursos : AoM.IInternalUpdate
	{
		readonly Dictionary<string, IRecurso> _data;

		/// <summary>
		/// Devuelve el valor de un recurso
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		public float? ValorRecurso (string nombre)
		{
			// Analysis disable RedundantExplicitNullableCreation
			IRecurso ret;
			var fRet = _data.TryGetValue (nombre, out ret) ? new float? (ret.Valor) : null;
			return fRet;
			// Analysis restore RedundantExplicitNullableCreation
		}

		/// <summary>
		/// Devuelve el recurso de nombre dado.
		/// </summary>
		/// <param name="nombre">Nombre del recurso</param>
		public IRecurso GetRecurso (string nombre)
		{
			return _data [nombre];
		}

		/// <summary>
		/// Agrega un recurso al sistema
		/// </summary>
		/// <param name="rec">Nuevo recurso.</param>
		public void Add (IRecurso rec)
		{
			_data.Add (rec.NombreÚnico, rec);
		}

		/// <summary>
		/// Remove the specified recurso
		/// </summary>
		/// <param name="rec">Recurso</param>
		public bool Remove (IRecurso rec)
		{
			return _data.Remove (rec.NombreÚnico);
		}

		/// <summary>
		/// Remove the specified recurso
		/// </summary>
		/// <param name="nombreRec">Nombre único del recurso</param>
		public bool Remove (string nombreRec)
		{
			return _data.Remove (nombreRec);
		}

		/// <summary>
		/// Runs the logic update
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void Update (float gameTime)
		{
			foreach (var x in _data.Values)
				x.Update (gameTime);
		}

		public override string ToString ()
		{
			var sb = new StringBuilder ();
			foreach (var x in _data)
				sb.AppendFormat ("[{0}]: [{1}]\n", x.Key, x.Value);
			return sb.ToString ();
		}

		public IEnumerable<IRecurso> Enumerar ()
		{
			return _data.Values;
		}

		/// <summary>
		/// </summary>
		public ManejadorRecursos ()
		{
			_data = new Dictionary<string, IRecurso> ();
		}

		/// <summary>
		/// </summary>
		/// <param name="recursos">Recursos</param>
		public ManejadorRecursos (IEnumerable<IRecurso> recursos)
			: this ()
		{
			foreach (var rec in recursos)
				Add (rec);
		}
	}
}