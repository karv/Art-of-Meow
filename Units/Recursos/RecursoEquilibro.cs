using System;
using Microsoft.Xna.Framework;

namespace Units.Recursos
{
	public class RecursoEquilibro : Recurso, IVisibleRecurso
	{
		#region Visibilidad

		float IVisibleRecurso.PctValue (float value)
		{
			return value;
		}

		bool IVisibleRecurso.Visible
		{
			get
			{
				return true;
			}
		}

		string IVisibleRecurso.TextureFill
		{
			get
			{
				return "pixel";
			}
		}

		Color IVisibleRecurso.FullColor
		{
			get
			{
				return Color.Blue;
			}
		}

		#endregion

		protected override string GetLongName ()
		{
			return "Equilibro";
		}

		protected override string GetShortName ()
		{
			return "Eq";
		}

		protected override string GetUniqueName ()
		{
			return ConstantesRecursos.Equilibrio;
		}

		public override void Update (float gameTime)
		{
			base.Update (gameTime);
			doRegen (gameTime);
		}

		void doRegen (float gameTime)
		{
			var ubValue = Regen * gameTime + Valor;
			Valor = Math.Min (1, ubValue);
		}

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

		public float Regen
		{ 
			get { return _regen.Valor; }
			set { _regen.Valor = value; }
		}

		readonly RegenParam _regen;
		readonly StatRecurso.ValorParám _valor;

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

