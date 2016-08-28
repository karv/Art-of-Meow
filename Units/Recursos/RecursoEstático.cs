using System;

namespace Units.Recursos
{
	public class RecursoEstático : IRecurso
	{
		public void Update (Microsoft.Xna.Framework.GameTime gameTime)
		{
		}

		public string Nombre { get; }

		float _valor;

		public float Valor
		{
			get{ return _valor; }
			set
			{
				_valor = value;
				if (_valor <= 0)
					AlLlegarCero?.Invoke (this, EventArgs.Empty);
			}
		}

		public IUnidad Unidad { get; }

		public event EventHandler AlLlegarCero;

		public RecursoEstático (string nombre, IUnidad unidad)
		{
			Nombre = nombre;
			Unidad = unidad;
		}

		public RecursoEstático (string nombre, IUnidad unidad, float valor)
			: this (nombre, unidad)
		{
			Valor = valor;
		}
	}
}