using System;

namespace Units.Recursos
{
	public class HpRecurso : IRecurso
	{
		public void Update (Microsoft.Xna.Framework.GameTime gameTime)
		{
		}

		public string NombreÚnico { get { return "hp"; } }

		public string NombreCorto { get { return "HP"; } }

		public string NombreLargo { get { return "HP"; } }

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

		public HpRecurso (IUnidad unidad)
		{
			Unidad = unidad;
		}
	}
}