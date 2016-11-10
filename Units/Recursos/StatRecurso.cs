using System;
using System.Diagnostics;
using AoM;

namespace Units.Recursos
{
	/// <summary>
	/// Recurso genérico de stats
	/// </summary>
	public class StatRecurso : Recurso
	{
		#region IUpdate implementation

		/// <summary>
		/// Updates the parameters, and manages regeneration
		/// </summary>
		public override void Update (float gameTime)
		{
			base.Update (gameTime);
			RegenMax (gameTime);
			RegenCurr (gameTime);
		}

		/// <summary>
		/// Realiza la regeneración del valor máximo
		/// </summary>
		protected virtual void RegenMax (float gameTime)
		{
			var diff = Base - Max;
			Max += diff * gameTime;
		}

		/// <summary>
		/// Realiza la regeneración del valor actual.
		/// </summary>
		protected virtual void RegenCurr (float gameTime)
		{
			var diff = Max - Valor;
			Valor += diff * gameTime;
		}

		#endregion

		#region IRecurso implementation

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		/// <value>The nombre único.</value>
		protected override string GetShortName ()
		{
			return NombreCorto;
		}

		/// <summary>
		/// Gets the long detailed name
		/// </summary>
		protected override string GetLongName ()
		{
			return NombreLargo;
		}

		/// <summary>
		/// Gets the unique name (mod resources)
		/// </summary>
		protected override string GetUniqueName ()
		{
			return NombreÚnico;
		}

		/// <summary>
		/// Nombre corto
		/// </summary>
		public string NombreCorto { get; set; }

		/// <summary>
		/// Nombre largo
		/// </summary>
		public string NombreLargo { get; set; }

		/// <summary>
		/// Nombre único
		/// </summary>
		public string NombreÚnico { get; }

		/// <summary>
		/// Devuelve el parámetro de valor actual
		/// </summary>
		public ValorParám ValorP { get; }

		/// <summary>
		/// Devuelve el parámetro de máximo actual
		/// </summary>
		public ValorParám MaxP { get; }

		/// <summary>
		/// Devuelve el parámetro base
		/// </summary>
		public ValorParám BaseP { get; }

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		/// <value>The valor.</value>
		public override float Valor
		{ 
			get { return ValorP.Valor; } 
			set { ValorP.Valor = Math.Min (value, Max); }
		}

		/// <summary>
		/// Devuelve o establece el máximo actual
		/// </summary>
		/// <value>The max.</value>
		public float Max
		{ 
			get { return MaxP.Valor; } 
			set
			{ 
				MaxP.Valor = Math.Min (value, Base); 
				ValorP.Valor = Math.Min (ValorP.Valor, MaxP.Valor);
			}
		}

		/// <summary>
		/// Devuelve o establece el valor base.
		/// </summary>
		/// <value>The base.</value>
		public float Base
		{ 
			get { return BaseP.Valor; } 
			set
			{ 
				BaseP.Valor = value;
				Max = Math.Min (value, MaxP.Valor);  // Esto actualiza también Value
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

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Recursos.StatRecurso"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Recursos.StatRecurso"/>.</returns>
		public override string ToString ()
		{
			return base.ToString () + string.Format ("{0}/{1}/{2}", Valor, Max, Base);
		}

		/// <param name="nombreÚnico">Nombre único.</param>
		/// <param name="unidad">Unidad.</param>
		public StatRecurso (string nombreÚnico, IUnidad unidad)
			: base (unidad)
		{
			NombreÚnico = nombreÚnico;
			ValorP = new ValorParám (this, "valor");
			MaxP = new ValorParám (this, "max");
			BaseP = new ValorParám (this, "base");
			Parámetros.Add (ValorP);
			Parámetros.Add (MaxP);
			Parámetros.Add (BaseP);
			BaseP.ValueMilestoneChanged += baseValueChanged;
		}

		void baseValueChanged (object sender, EventArgs e)
		{
			Debug.WriteLine (string.Format (
				"Jugador: {2}\t\tstat base {0} changed to {1}.",
				NombreÚnico,
				BaseP.Valor,
				Unidad));
		}



		/// <summary>
		/// Representa un parámetro de recurso de un sólo valor
		/// </summary>
		public class ValorParám : IParámetroRecurso
		{
			void IParámetroRecurso.ReceiveExperience (float exp)
			{
				Valor += exp;
				Debug.WriteLine (
					string.Format (
						"{0}/{3} recibe exp {1}.\nNuevo val: {2}",
						NombreÚnico,
						exp,
						Valor, 
						Recurso.NombreÚnico),
					"Experiencia");
			}

			void IInternalUpdate.Update (float gameTime)
			{
			}

			/// <summary>
			/// Devuelve el recurso que contiene este parámetro
			/// </summary>
			public IRecurso Recurso { get; }

			/// <summary>
			/// Devuelve el nombre único de este parámetro dentro de <see cref="Recurso"/>
			/// </summary>
			public string NombreÚnico { get; }

			float valor;

			/// <summary>
			/// Devuelve o establece el valor del parámetro
			/// </summary>
			public float Valor
			{
				get
				{
					return valor;
				}
				set
				{
					var int_before = (int)valor;
					valor = value;
					var int_after = (int)valor;
					if (int_before != int_after)
						ValueMilestoneChanged?.Invoke (this, EventArgs.Empty);
				}
			}

			/// <summary>
			/// Ocurre al cambiar el valor entero del valor
			/// </summary>
			public event EventHandler ValueMilestoneChanged;

			/// <summary>
			/// </summary>
			public override string ToString ()
			{
				return string.Format (
					"[ValorParám: Recurso={0}, NombreÚnico={1}, Valor={2}]",
					Recurso,
					NombreÚnico,
					Valor);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="Units.Recursos.StatRecurso.ValorParám"/> class.
			/// </summary>
			/// <param name="rec">Recurso</param>
			/// <param name="nombre">Nombre único</param>
			public ValorParám (IRecurso rec, string nombre)
			{
				Recurso = rec;
				NombreÚnico = nombre;
			}
		}
	}
}