using System;
using AoM;
using Cells;
using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Screens;
using Skills;
using Units;
using Units.Order;
using Units.Recursos;
using Units.Skills;
using OpenTK.Graphics.OpenGL;
using System.Text;
using System.Diagnostics;

namespace Items.Declarations.Equipment.Skills
{
	/// <summary>
	/// Skill de ataque de rango de esta clase.
	/// </summary>
	public class RangedDamage : ISkill
	{
		static IEffect[] effectMaker (IUnidad user, IUnidad target)
		{
			if (target == null)
				throw new ArgumentNullException ("target");
			if (user == null)
				throw new ArgumentNullException ("user");
			
			var chance = HitDamageCalculator.GetPctHit (
				             user,
				             target,
				             ConstantesRecursos.CertezaRango,
				             ConstantesRecursos.EvasiónRango);
			var dmg = HitDamageCalculator.Damage (
				          user,
				          target,
				          ConstantesRecursos.Fuerza,
				          ConstantesRecursos.Fuerza);
			var ret = new ChangeRecurso (user, target, ConstantesRecursos.HP, -dmg)
			{ Chance = chance };
			return new IEffect[] { ret };
		}

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		/// <value>The last generated instance.</value>
		public SkillInstance LastGeneratedInstance { get; protected set; }

		/// <summary>
		/// Build a skill instance
		/// </summary>
		/// <param name="user">Caster</param>
		public void GetInstance (IUnidad user)
		{
			var dialSer = new Moggle.Screens.Dials.ScreenDialSerial ();

			var selScr = new SelectTargetScreen (Program.MyGame, user.Grid);
			selScr.GridSelector.CursorPosition = user.Grid.GetClosestEnemy (user);

			var infoBox = new EtiquetaMultiLínea (selScr)
			{
				MaxWidth = 200,
				TextColor = Color.White,
				TopLeft = new Point (500, 200),
				UseFont = "Fonts//small",
				Texto = "Info",
				BackgroundColor = Color.Black
			};
			selScr.AddComponent (infoBox);

			dialSer.AddRequest (selScr);
			selScr.GridSelector.CursorMoved += delegate(object sender, EventArgs e)
			{
				// Se cambió la selección; volver a calcular skill
				var pt = selScr.GridSelector.CursorPosition;
				var tg = user.Grid [pt].GetAliveUnidadHere ();
				if (tg == null)
					return;
				var effs = effectMaker (user, tg);
				var infoStrBuilding = new StringBuilder ();
				foreach (var eff in effs)
					infoStrBuilding.AppendLine (" * " + eff.DetailedInfo ());
				
				infoBox.Texto = infoStrBuilding.ToString ();
				infoBox.MaxWidth = infoBox.MaxWidth; // TODO: Eliminar cuando se use Moggle 0.11.1

				//LastGeneratedInstance.AddEffect (eff);

				//LastGeneratedInstance.Execute ();
			};

			dialSer.HayRespuesta += delegate(object sender, object [] e)
			{
				var pt = (Point)e [0];
				var tg = user.Grid [pt].GetAliveUnidadHere ();
				LastGeneratedInstance = new SkillInstance (this, tg);
				var effs = effectMaker (user, tg);
				foreach (var eff in effs)
					LastGeneratedInstance.AddEffect (eff);

				LastGeneratedInstance.Execute ();
			};

			selScr.HayRespuesta += (sender, e) => Executed?.Invoke (
				this,
				EventArgs.Empty);
			dialSer.Executar (Program.MyGame.ScreenManager.ActiveThread);	
		}

		/// <summary>
		/// Causa el efecto en un punto
		/// </summary>
		protected virtual void DoEffect (Units.IUnidad user, Point? targetPoint)
		{
			if (targetPoint.HasValue)
			{
				var targ = user.Grid.GetCell (targetPoint.Value).GetAliveUnidadHere ();
				if (targ != null)
					DoEffect (user, targ);
				
				// Se invoca Execute aunque no haga nada
				Executed?.Invoke (this, EventArgs.Empty);
			}
		}

		static void DoEffect (Units.IUnidad user, Units.IUnidad target)
		{
			var cert = user.Recursos.GetRecurso (ConstantesRecursos.CertezaRango).Valor;
			var eva = target.Recursos.GetRecurso (ConstantesRecursos.CertezaRango).Valor;
			var pctHit = eva < cert ? 0.8 : 0.5;
			var damage = user.Recursos.ValorRecurso (ConstantesRecursos.Destreza) * 0.35f;
			var _r = new Random ();
			if (_r.NextDouble () < pctHit)
				user.EnqueueOrder (new MeleeDamageOrder (user, target, damage));
			
			user.EnqueueOrder (new CooldownOrder (
				user,
				1f / user.Recursos.ValorRecurso (ConstantesRecursos.Destreza)));

			// Asignación de stats
			user.Exp.AddAssignation (
				user.Recursos.GetRecurso (ConstantesRecursos.CertezaRango),
				0.1f);

			target.Exp.AddAssignation (
				target.Recursos.GetRecurso (ConstantesRecursos.EvasiónRango),
				0.1f);
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		/// <param name="user">User</param>
		public bool IsCastable (IUnidad user)
		{
			// TODO: Debe consumir(requerir) ¿ammo?
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible the specified user.
		/// </summary>
		/// <param name="user">User.</param>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Occurs when the eexecution finishes completly
		/// </summary>
		public event EventHandler Executed;

		#region Contenido

		/// <summary>
		/// Gets or sets the name of the texture.
		/// </summary>
		/// <value>The name of the texture.</value>
		public string TextureName { get; set; }

		/// <summary>
		/// Gets the icon.
		/// </summary>
		/// <value>The icon.</value>
		protected Texture2D Icon { get; private set; }

		void IComponent.AddContent ()
		{
			Program.MyGame.Contenido.AddContent (TextureName);
		}

		void IComponent.InitializeContent ()
		{
			Icon = Program.MyGame.Contenido.GetContent<Texture2D> (TextureName);
		}

		#endregion

		void IGameComponent.Initialize ()
		{
		}

		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (Icon, destinationRectangle: rect, layerDepth: Depths.SkillIcon);
		}
	}
}