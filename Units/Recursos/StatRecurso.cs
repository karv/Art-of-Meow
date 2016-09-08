using System;
using Microsoft.Xna.Framework;

namespace Units.Recursos
{
	public class StatRecurso : IRecurso
	{
		#region IUpdate implementation

		void MonoGame.Extended.IUpdate.Update (GameTime gameTime)
		{
			regenMax (gameTime);
			regenCurr (gameTime);
		}

		void regenMax (GameTime gameTime)
		{
			var diff = Base - Max;
			Max += diff * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		void regenCurr (GameTime gameTime)
		{
			var diff = Max - Valor;
			Valor += diff * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		#endregion

		#region IRecurso implementation

		public string NombreÚnico { get; }

		public string NombreCorto { get; set; }

		public string NombreLargo { get; set; }

		readonly float [] currMaxNormal = new float[3];

		public float Valor
		{ 
			get { return currMaxNormal [0]; } 
			set	{ currMaxNormal [0] = Math.Min (value, Max); }
		}

		public float Max
		{ 
			get { return currMaxNormal [1]; } 
			set
			{ 
				currMaxNormal [1] = Math.Min (value, Base); 
				currMaxNormal [0] = Math.Min (currMaxNormal [0], currMaxNormal [1]);
			}
		}

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

		public float TasaRecuperaciónNormal { get; set; }

		public float TasaRecuperaciónMax { get; set; }

		public IUnidad Unidad { get; }

		#endregion

		public StatRecurso (string nombreÚnico, IUnidad unidad)
		{
			NombreÚnico = nombreÚnico;
			Unidad = unidad;
		}
	}
}