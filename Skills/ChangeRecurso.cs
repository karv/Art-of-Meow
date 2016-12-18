using System;
using System.Diagnostics;
using AoM;
using Componentes;
using Debugging;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Screens;
using Skills;
using Units;
using Units.Recursos;

namespace Skills
{
	/// <summary>
	/// Un efecto de cambio de recurso en un target
	/// </summary>
	public class ChangeRecurso : Effect, ITargetEffect
	{
		/// <summary>
		/// Se invoca cuando acierta
		/// </summary>
		protected override void WhenHit ()
		{
			DoRun ();

			if (ShowDeltaLabel)
				ShowLabel ();
		}

		/// <summary>
		/// Se invoca cuando falla
		/// </summary>
		protected override void WhenMiss ()
		{
			if (ShowDeltaLabel)
				ShowLabel ();
		}

		/// <summary>
		/// Devuelve un <c>string</c> de una línea que describe este efecto como infobox
		/// </summary>
		public override string DetailedInfo ()
		{
			if (Parámetro == null)
				return string.Format (
					"{3}: {0}'s {1} changed by {2}.",
					Target,
					TargetRecurso.NombreLargo,
					DeltaValor,
					Chance);
			return string.Format (
				"{4}: {0}'s {1}'s {2} changed by {3}.",
				Target,
				TargetRecurso.NombreLargo,
				Parámetro.NombreÚnico,
				DeltaValor,
				Chance);
		}

		/// <summary>
		/// Runs the effect
		/// </summary>
		protected void DoRun ()
		{
			
			if (Parámetro == null)
				// Efecto es en recurso
				TargetRecurso.Valor += DeltaValor;
			else
				// Efecto es en parámetro
				Parámetro.Valor += DeltaValor;

			Debug.WriteLine (ToString (), DebugCategories.ResourceAffected);

			if (TargetRecurso is RecursoHP && TargetRecurso.Valor == 0)
				OnKill ();
		}

		/// <summary>
		/// Muestra al jugador/usuario el daño causado
		/// </summary>
		protected void ShowLabel ()
		{
			if (DeltaValor == 0)
				return;
			var scr = Program.MyGame.ScreenManager.ActiveThread.ClosestOfType<MapMainScreen> () as MapMainScreen;
			var txt = Result == EffectResultEnum.Hit ?
				Math.Abs (Math.Truncate (DeltaValor * 10) / 10).ToString () :
				"...."; // TODO Renombrar a "miss" cuando tenga el content
			var label = new VanishingLabel (
				            scr,
				            txt,
				            TimeSpan.FromMilliseconds (900));
			scr.AddComponent (label);
			label.Initialize ();

			label.FontName = "Fonts//damage";
			(label as IComponent).LoadContent (scr.Content);

			label.Centro = scr.GridControl.CellSpotLocation (Target.Location).ToVector2 ();
			label.ColorInicial = LabelColor;
		}

		/// <summary>
		/// Se invoca cuando este efecto asesina a una unidad
		/// </summary>
		protected void OnKill ()
		{
			var expGetter = Agent as IUnidad;
			if (expGetter != null)
			{
				var exp = Target.GetExperienceValue ();
				expGetter.Exp.ExperienciaAcumulada += exp;
				Debug.WriteLine (string.Format (
					"{0} kills {1}.\nReceving {2} exp",
					Agent,
					Target,
					exp), DebugCategories.Experience);
			}
		}

		/// <summary>
		/// El recurso que es afectado.
		/// El valor de este recurso no es afectado, al menos que <see cref="Parámetro"/> sea <c>null</c>
		/// </summary>
		public IRecurso TargetRecurso { get; }

		/// <summary>
		/// Devuelve el parámetro del recurso afectado; 
		/// si es <c>null</c>, se afecta directamente a <see cref="TargetRecurso"/>
		/// </summary>
		/// <value>The parámetro.</value>
		public IParámetroRecurso Parámetro { get; }

		/// <summary>
		/// Devuelve la unidad que es afectada
		/// </summary>
		public IUnidad Target { get; }

		ITarget ITargetEffect.Target { get { return Target; } }

		/// <summary>
		/// Devuelve la diferencia de valor que causa el recurso
		/// </summary>
		public float DeltaValor { get; }

		/// <summary>
		/// Devuelve o establece si debe de ser mostrada una etiqueta con el delta causado
		/// </summary>
		public bool ShowDeltaLabel { get; set; }

		/// <summary>
		/// Cuando <see cref="ShowDeltaLabel"/> es <c>true</c>, este valor determina el color de la equiqueta.
		/// </summary>
		/// <seealso cref="ShowDeltaLabel"/>
		public Color LabelColor { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Skills.ChangeRecurso"/>.
		/// </summary>
		public override string ToString ()
		{
			if (Parámetro == null)
				return string.Format ("{0} += {1}", TargetRecurso.NombreCorto, DeltaValor);
			return string.Format (
				"{0}.{1} += {2}",
				TargetRecurso.NombreCorto,
				Parámetro.NombreÚnico,
				DeltaValor);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.ChangeRecurso"/> class.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="target">Target.</param>
		/// <param name="recNombre">Nombre del recurso</param>
		/// <param name="recParámetro">Nombre del parámetro</param>
		/// <param name="deltaValor">Cambio de valor</param>
		/// <param name = "chance">PRobabilidad de que HIT</param>
		public ChangeRecurso (IEffectAgent agent,
		                      IUnidad target,
		                      string recNombre,
		                      string recParámetro, 
		                      float deltaValor,
		                      double chance = 1)
			: base (agent, chance)
		{
			Target = target;
			TargetRecurso = target.Recursos.GetRecurso (recNombre);
			Parámetro = TargetRecurso.ValorParámetro (recParámetro);
			DeltaValor = deltaValor;
			ShowDeltaLabel = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Skills.ChangeRecurso"/> class.
		/// </summary>
		/// <param name="agent">Agent.</param>
		/// <param name="target">Target.</param>
		/// <param name="recNombre">Nombre del recurso</param>
		/// <param name="deltaValor">Cambio de valor</param>
		/// <param name = "chance">PRobabilidad de que HIT</param>
		public ChangeRecurso (IEffectAgent agent,
		                      IUnidad target,
		                      string recNombre, 
		                      float deltaValor,
		                      double chance = 1)
			: base (agent, chance)
		{
			Target = target;
			TargetRecurso = target.Recursos.GetRecurso (recNombre);
			DeltaValor = deltaValor;
			ShowDeltaLabel = true;
			LabelColor = Color.Red;
		}
	}
}