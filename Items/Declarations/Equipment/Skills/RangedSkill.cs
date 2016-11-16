using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units.Skills;
using Screens;
using System;
using Units.Order;
using Units.Recursos;

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
			//var scr = Program.MyGame.CurrentScreen as Moggle.Screens.DialScreen;
			//scr?.Salir ();
			var executionScreen = new SelectTargetScreen (Program.MyGame, user.Grid);
			Program.MyGame.ScreenManager.ActiveThread.TerminateLast (); // Terminar la de selección de skill
			executionScreen.Selected += delegate
			{
				DoEffect (user, executionScreen.GridSelector.CursorPosition);
				Executed?.Invoke (this, EventArgs.Empty);
			};
			executionScreen.Execute (ScreenExt.DialogOpt);

			// TODO: Seleccionar target
		}

		/// <summary>
		/// Causa el efecto en un punto
		/// </summary>
		protected virtual void DoEffect (Units.IUnidad user, Point targetPoint)
		{
			var targ = user.Grid.GetCell (targetPoint).GetAliveUnidadHere ();
			DoEffect (user, targ);
		}

		static void DoEffect (Units.IUnidad user, Units.IUnidad target)
		{
			// TODO
			var damage = user.Recursos.ValorRecurso (ConstantesRecursos.Destreza) * 0.35f;
			user.EnqueueOrder (new MeleeDamageOrder (user, target, damage));
			user.EnqueueOrder (new CooldownOrder (
				user,
				1f / user.Recursos.ValorRecurso (ConstantesRecursos.Destreza)));
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