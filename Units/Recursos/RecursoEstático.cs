using System;

namespace Units.Recursos
{
	/// <summary>
	/// Recurso de valor 'fijo'
	/// </summary>
	public class RecursoEstático : IRecurso
	{
		public void Update (float gameTime)
		{
		}

		IParámetroRecurso IRecurso.ValorParámetro (string paramName)
		{
			throw new Exception ("Cannot get value of this kind of resource.");
		}

		public IRecurso Recurso { get { return this; } }

		public string NombreCorto { get; set; }

		public string NombreLargo { get; set; }

		public string NombreÚnico { get; }

		public float Valor { get; set; }

		public IUnidad Unidad { get; }

		public RecursoEstático (string nombre, IUnidad unidad)
		{
			NombreÚnico = nombre;
			Unidad = unidad;
		}

		public RecursoEstático (string nombre, IUnidad unidad, float valor)
			: this (nombre, unidad)
		{
			Valor = valor;
		}
	}
}