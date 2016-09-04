using System;

namespace Units.Recursos
{
	public class RecursoHP : IRecurso
	{
		public void Update (Microsoft.Xna.Framework.GameTime gameTime)
		{
		}

		public string NombreÚnico { get { return "hp"; } }

		public string NombreCorto { get { return "HP"; } }

		public string NombreLargo { get { return "Hit points"; } }

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

		public float RelativeHp { get { return _valor / Max; } }

		public void Fill ()
		{
			_valor = Max;
		}

		public void Empty ()
		{
			_valor = 0;
		}

		public IUnidad Unidad { get; }

		public RecursoHP (IUnidad unidad)
		{
			Unidad = unidad;
		}
	}
}