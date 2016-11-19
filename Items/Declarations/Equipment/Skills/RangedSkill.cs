using System;
using AoM;
using Cells;
using Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units.Order;
using Units.Recursos;
using Units.Skills;
using Skills;
using Units;
using Screens;

namespace Items.Declarations.Equipment.Skills
{
	/// <summary>
	/// Skill de ataque de rango de esta clase.
	/// </summary>
	public class RangedDamage : ISkill
	{
		/// <summary>
		/// Ejecuta
		/// </summary>
		/// <param name="user">Usuario</param>
		public void Execute (Units.IUnidad user)
		{
			// Calcular la posisicón inicial del cursor: la del enemigo más cercano
			SelectorController.Run (
				user.Grid,
				z => DoEffect (user, z),
				user.Grid.GetClosestEnemy (user),
				true);
		}

		static IEffect[] effectMaker (IUnidad user, IUnidad target)
		{
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

		public SkillInstance LastGeneratedInstance { get; protected set; }

		public void GetInstance (IUnidad user)
		{
			WaitResponseScreen<Point, object> fakeScreen;
			fakeScreen = new WaitResponseScreen<Point, object> ();

			var selScr = new SelectTargetScreen (Program.MyGame, user.Grid);
			selScr.GridSelector.CursorPosition = user.Grid.GetClosestEnemy (user);

			fakeScreen.AddRequestArgument (
				selScr, 
				z => user.Grid.GetCell (z).GetAliveUnidadHere ());

			fakeScreen.Response += delegate(object sender, object [] e)
			{
				var tg = e [0] as IUnidad; // target
				LastGeneratedInstance = new SkillInstance (this, tg);
				var effs = effectMaker (user, tg);
				foreach (var eff in effs)
					LastGeneratedInstance.AddEffect (eff);

				LastGeneratedInstance.Execute ();
			};

			fakeScreen.Run ();
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
		public bool IsCastable (Units.IUnidad user)
		{
			// TODO: Debe consumir(requerir) ¿ammo?
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible the specified user.
		/// </summary>
		/// <param name="user">User.</param>
		public bool IsVisible (Units.IUnidad user)
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