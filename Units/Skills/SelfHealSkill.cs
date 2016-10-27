using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units.Recursos;

namespace Units.Skills
{
	/// <summary>
	/// Skill that allows the user to heal ?-self
	/// </summary>
	public class SelfHealSkill : ISkill
	{
		#region ISkill implementation

		const float _heal = 10f;
		const float _mpCost = 3f;

		/// <summary>
		/// Executes this skill
		/// </summary>
		/// <param name="user">The caster</param>
		public void Execute (IUnidad user)
		{
			var hpRec = user.Recursos.GetRecurso (ConstantesRecursos.HP);
			hpRec.Valor += 10;
		}

		/// <summary>
		/// Determines whether this skill is castable by the specified user.
		/// </summary>
		/// <returns><c>true</c> if this skill is castable by the specified user; otherwise, <c>false</c>.</returns>
		/// <param name="user">User</param>
		public bool IsCastable (IUnidad user)
		{
			return true;
		}

		/// <summary>
		/// Determines whether this instance is visible to the specified <see cref="IUnidad"/>
		/// </summary>
		/// <returns><c>true</c> if this instance is visible by the specified user; otherwise, <c>false</c></returns>
		/// <param name="user">User</param>
		public bool IsVisible (IUnidad user)
		{
			return true;
		}

		#endregion

		#region IComponent implementation

		Texture2D _texture;

		const string TextureName = "person";

		/// <summary>
		/// Loads the icon texture
		/// </summary>
		/// <param name="manager">Manager.</param>
		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			_texture = manager.Load<Texture2D> (TextureName);
		}

		/// <summary>
		/// </summary>
		void IComponent.UnloadContent ()
		{
		}

		#endregion

		#region IDisposable implementation

		void IDisposable.Dispose ()
		{
		}

		#endregion

		#region IGameComponent implementation

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
		}

		#endregion

		#region IDibujable implementation

		/// <summary>
		/// Draws the icon on a rectangle
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="rect">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (_texture, rect, Color.White);
		}

		#endregion
	}
}