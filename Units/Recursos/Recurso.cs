using System;
using System.Collections.Generic;

namespace Units.Recursos
{
	public abstract class Recurso : IRecurso
	{
		protected List<IParámetroRecurso> Parámetros;

		public IEnumerable<IParámetroRecurso> EnumerateParameters ()
		{
			return Parámetros;
		}

		#region IRecurso implementation

		public IParámetroRecurso ValorParámetro (string paramName)
		{
			foreach (var p in Parámetros)
				if (p.NombreÚnico == paramName)
					return p;
			throw new Exception ();
		}

		public abstract float Valor { get; set; }

		protected abstract string GetShortName ();

		protected abstract string GetLongName ();

		protected abstract string GetUniqueName ();

		string IRecurso.NombreCorto { get { return GetShortName (); } }

		string IRecurso.NombreLargo { get { return GetLongName (); } }

		string IRecurso.NombreÚnico { get { return GetUniqueName (); } }

		public IUnidad Unidad { get; }

		#endregion

		#region IInternalUpdate implementation

		public virtual void Update (float gameTime)
		{
			foreach (var par in Parámetros)
				par.Update (gameTime);
		}

		#endregion

		public override string ToString ()
		{
			return string.Format (
				"[Recurso: Parámetros={0}, Valor={1}, NombreÚnico={2}, Unidad={3}]",
				Parámetros,
				Valor,
				GetUniqueName (),
				Unidad);
		}

		protected Recurso (IUnidad unidad)
		{
			Unidad = unidad;
			Parámetros = new List<IParámetroRecurso> ();
		}
	}
}