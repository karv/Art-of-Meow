using System;

namespace Units.Recursos
{
	public class HpRecurso : IRecurso
	{
		public void Update (Microsoft.Xna.Framework.GameTime gameTime)
		{
		}

		public string NombreÚnico { get; }

		public string NombreCorto { get; }

		public string NombreLargo { get; }

		float _valor;

		public float Valor
		{
			get { return _valor; }
			set
			{
				_valor = Math.Min (Math.Max (value, 0), Max);
			}
		}

		public float Max { get; set; }

		public IUnidad Unidad { get; }

		public HpRecurso (string nombreÚnico, IUnidad unidad)
		{
			NombreÚnico = nombreÚnico;
			Unidad = unidad;
		}
	}
}