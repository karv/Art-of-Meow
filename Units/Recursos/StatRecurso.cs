using System;
using Microsoft.Xna.Framework;

namespace Units.Recursos
{
	/// <summary>
	/// Recurso genérico de stats
	/// </summary>
	public class StatRecurso : IRecurso
	{
		#region IUpdate implementation

		void MonoGame.Extended.IUpdate.Update (GameTime gameTime)
		{
			RegenMax (gameTime);
			RegenCurr (gameTime);
		}

		/// <summary>
		/// Realiza la regeneración del valor máximo
		/// </summary>
		protected virtual void RegenMax (GameTime gameTime)
		{
			var diff = Base - Max;
			Max += diff * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		/// <summary>
		/// Realiza la regeneración del valor actual.
		/// </summary>
		protected virtual void RegenCurr (GameTime gameTime)
		{
			var diff = Max - Valor;
			Valor += diff * (float)gameTime.ElapsedGameTime.TotalSeconds;
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

		readonly float [] currMaxNormal = new float[3];

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		/// <value>The valor.</value>
		public float Valor
		{ 
			get { return currMaxNormal [0]; } 
			set	{ currMaxNormal [0] = Math.Min (value, Max); }
		}

		/// <summary>
		/// Devuelve o establece el máximo actual
		/// </summary>
		/// <value>The max.</value>
		public float Max
		{ 
			get { return currMaxNormal [1]; } 
			set
			{ 
				currMaxNormal [1] = Math.Min (value, Base); 
				currMaxNormal [0] = Math.Min (currMaxNormal [0], currMaxNormal [1]);
			}
		}

		/// <summary>
		/// Devuelve o establece el valor base.
		/// </summary>
		/// <value>The base.</value>
		public float Base
		{ 
			get { return currMaxNormal [2]; } 
			set
			{ 
				currMaxNormal [2] = value;
				currMaxNormal [1] = Math.Min (value, currMaxNormal [1]); 
				currMaxNormal [0] = Math.Min (currMaxNormal [0], currMaxNormal [1]);
			}

		}

		/// <summary>
		/// Devuelve o establece la tasa de recuparación del valor normal respecto al valor máximo
		/// </summary>
		public float TasaRecuperaciónNormal { get; set; }

		/// <summary>
		/// Devuelve o establece la tasa de recuparación del valor máximo respecto al valor base
		/// </summary>
		public float TasaRecuperaciónMax { get; set; }

		/// <summary>
		/// Unidad que posee este recurso.
		/// </summary>
		/// <value>The unidad.</value>
		public IUnidad Unidad { get; }

		#endregion

		/// <param name="nombreÚnico">Nombre único.</param>
		/// <param name="unidad">Unidad.</param>
		public StatRecurso (string nombreÚnico, IUnidad unidad)
		{
			NombreÚnico = nombreÚnico;
			Unidad = unidad;
		}
	}
}