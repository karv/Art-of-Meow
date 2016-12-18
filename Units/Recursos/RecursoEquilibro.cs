using System;
using AoM;
using Microsoft.Xna.Framework;

namespace Units.Recursos
{
	/// <summary>
	/// Recurso equilibro
	/// </summary>
	public class RecursoEquilibro : Recurso, IVisibleRecurso
	{
		readonly Color FillColor = Color.Blue;

		void Moggle.Controles.IDibujable.Draw (Microsoft.Xna.Framework.Graphics.SpriteBatch bat,
		                                       Rectangle rect)
		{
			var text = Juego.Textures.SolidTexture;

			/* Horizontal*/
			var fullRect = new Rectangle (
				               rect.Location,
				               new Point ((int)(Valor * rect.Width), rect.Height));

			bat.Draw (text, fullRect, FillColor);
		}

		void Moggle.Controles.IComponent.LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
		}

		void IGameComponent.Initialize ()
		{
		}

		
		/// <summary>
		/// El valor en el que se reduce con acada acción
		/// </summary>
		public const float ReduceValue = 0.1f;

		#region Visibilidad

		bool IVisibleRecurso.Visible
		{
			get
			{
				return true;
			}
		}

		#endregion

		/// <summary>
		/// Gets the detailed name
		/// </summary>
		/// <returns>The long name.</returns>
		protected override string GetLongName ()
		{
			return "Equilibro";
		}

		/// <summary>
		/// Gets the short name
		/// </summary>
		/// <returns>The short name.</returns>
		protected override string GetShortName ()
		{
			return "Eq";
		}

		/// <summary>
		/// Gets the unique name
		/// </summary>
		/// <returns>The unique name.</returns>
		protected override string GetUniqueName ()
		{
			return ConstantesRecursos.Equilibrio;
		}

		/// <summary>
		/// Updates the object
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public override void Update (float gameTime)
		{
			base.Update (gameTime);
			doRegen (gameTime);
		}

		void doRegen (float gameTime)
		{
			var ubValue = Regen / 2 * gameTime + Valor;
			Valor = Math.Min (1, ubValue);
		}

		/// <summary>
		/// Valor actual del recurso.
		/// </summary>
		public override float Valor
		{
			get
			{
				return _valor.Valor;
			}
			set
			{
				_valor.Valor = value;
			}
		}

		/// <summary>
		/// El valor de la regeneración
		/// </summary>
		/// <value>The regen.</value>
		public float Regen
		{ 
			get { return _regen.Valor; }
			set { _regen.Valor = value; }
		}

		readonly RegenParam _regen;
		readonly StatRecurso.ValorParám _valor;

		/// <summary>
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		public RecursoEquilibro (IUnidad unidad)
			: base (unidad)
		{
			_regen = new RegenParam (this);
			_valor = new StatRecurso.ValorParám (this, "value");
			Parámetros.Add (_regen);
			Parámetros.Add (_valor);
		}

		class RegenParam : IParámetroRecurso
		{
			public void ReceiveExperience (float exp)
			{
				throw new NotImplementedException ();
			}

			public void Update (float gameTime)
			{
			}

			public IRecurso Recurso { get; }

			public string NombreÚnico { get { return "regen"; } }

			public float Valor { get; set; }

			public RegenParam (IRecurso recBase)
			{
				Recurso = recBase;
			}
		}
	}
}