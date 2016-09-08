using System;

namespace Units.Recursos
{
	public class RecursoEstático : IRecurso
	{
		public void Update (float gameTime)
		{
		}

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