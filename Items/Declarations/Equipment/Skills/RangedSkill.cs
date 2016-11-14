using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units.Skills;
using System.Threading;
using Screens;
using System.Diagnostics;

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
			var executionScreen = new SelectTargetScreen (Program.MyGame, user.Grid);
			executionScreen.AlTerminar += delegate
			{
				Debugger.Break ();
			};
			executionScreen.Ejecutar ();

			// TODO: Seleccionar target
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