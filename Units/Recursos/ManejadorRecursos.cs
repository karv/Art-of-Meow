using System;
using System.Collections.Generic;
using System.Text;
using Units.Buffs;

namespace Units.Recursos
{
	/// <summary>
	/// Clase que sirve como medio entre un <see cref="IUnidad"/> y sus <see cref="IRecurso"/>
	/// </summary>
	public class ManejadorRecursos : AoM.IInternalUpdate
	{
		readonly Dictionary<string, IRecurso> _data;

		/// <summary>
		/// Gets the unidad.having this manager
		/// </summary>
		public IUnidad Unidad { get; }

		/// <summary>
		/// Devuelve el valor de un recurso
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		public float ValorRecursoBase (string nombre)
		{
			IRecurso ret;
			if (_data.TryGetValue (nombre, out ret))
				return ret.Valor;
			throw new Exception ("Recurso inexistente: " + nombre);
		}

		/// <summary>
		/// Devuelve el valor final de un recurso 
		/// </summary>
		public float ValorRecurso (string nombre)
		{
			var ret = ValorRecursoBase (nombre) + RecursoExtra (nombre);
			return ret;
		}

		/// <summary>
		/// Devuelve el valor de recursos modificado por Buffs.
		/// </summary>
		/// <returns>The extra.</returns>
		/// <param name="nombre">Nombre.</param>
		public float RecursoExtra (string nombre)
		{
			var ret = 0f;
			if (nombre == "hp.max")
				Console.Write ("");
			foreach (var buff in Unidad.Buffs.BuffOfType<IStatsBuff> ())
				ret += buff.StatDelta (nombre);
			return ret;
		}

		/// <summary>
		/// Devuelve el recurso de nombre dado.
		/// </summary>
		/// <param name="nombre">Nombre del recurso</param>
		public IRecurso GetRecurso (string nombre)
		{
			IRecurso ret;
			if (_data.TryGetValue (nombre, out ret))
				return ret;
			ret = new StatRecurso (nombre, Unidad);
			_data.Add (nombre, ret);
			return ret;
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

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Recursos.ManejadorRecursos"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Recursos.ManejadorRecursos"/>.</returns>
		public override string ToString ()
		{
			var sb = new StringBuilder ();
			foreach (var x in _data)
				sb.AppendFormat (
					"[{0}]: {2}\t\t [{1}]\n",
					x.Key,
					x.Value,
					ValorRecurso (x.Key));
			return sb.ToString ();
		}

		/// <summary>
		/// Enumerate the resources
		/// </summary>
		public IEnumerable<IRecurso> Enumerate ()
		{
			return _data.Values;
		}

		/// <summary>
		/// </summary>
		public ManejadorRecursos (IUnidad unid)
		{
			_data = new Dictionary<string, IRecurso> ();
			Unidad = unid;
		}

		/// <summary>
		/// </summary>
		/// <param name="unid">Unidad</param>
		/// <param name="recursos">Recursos</param>
		public ManejadorRecursos (IUnidad unid, IEnumerable<IRecurso> recursos)
			: this (unid)
		{
			foreach (var rec in recursos)
				Add (rec);
		}
	}
}